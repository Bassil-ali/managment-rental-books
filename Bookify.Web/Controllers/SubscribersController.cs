using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = AppRoles.Reception)]
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private readonly IImageService _imageService;

        public SubscribersController(ApplicationDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SearchFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subscriber = _context.Subscribers
                            .SingleOrDefault(s => 
                                    s.Email == model.Value
                                || s.NationalId == model.Value
                                || s.MobileNumber == model.Value);

            var viewModel = _mapper.Map<SubscriberSearchResultViewModel>(subscriber);

            return PartialView("_Result", viewModel);
        }

        public IActionResult Details(int id)
        {
            var subscriber = _context.Subscribers
                .Include(s => s.Governorate)
                .Include(s => s.Area)
                .SingleOrDefault(s => s.Id == id);

            if (subscriber is null)
                return NotFound();

            var viewModel = _mapper.Map<SubscriberViewModel>(subscriber);

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = PopulateViewModel();
            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriberFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var Subscriber = _mapper.Map<Subscriber>(model);

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
            var imagePath = "/images/Subscribers";

            var (isUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, imagePath, hasThumbnail: true);

            if (!isUploaded)
            {
                ModelState.AddModelError("Image", errorMessage!);
                return View("Form", PopulateViewModel(model));
            }

            Subscriber.ImageUrl = $"{imagePath}/{imageName}";
            Subscriber.ImageThumbnailUrl = $"{imagePath}/thumb/{imageName}";
            Subscriber.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            _context.Add(Subscriber);
            _context.SaveChanges();

            //TODO: Send welcome email

            return RedirectToAction(nameof(Details), new { id = Subscriber.Id });
        }

        public IActionResult Edit(int id)
        {
            var Subscriber = _context.Subscribers.Find(id);

            if (Subscriber is null)
                return NotFound();

            var model = _mapper.Map<SubscriberFormViewModel>(Subscriber);
            var viewModel = PopulateViewModel(model);

            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubscriberFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var Subscriber = _context.Subscribers.Find(model.Id);

            if (Subscriber is null)
                return NotFound();

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(Subscriber.ImageUrl))
                    _imageService.Delete(Subscriber.ImageUrl, Subscriber.ImageThumbnailUrl);

                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var imagePath = "/images/Subscribers";

                var (isUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, imagePath, hasThumbnail: true);

                if (!isUploaded)
                {
                    ModelState.AddModelError("Image", errorMessage!);
                    return View("Form", PopulateViewModel(model));
                }

                model.ImageUrl = $"{imagePath}/{imageName}";
                model.ImageThumbnailUrl = $"{imagePath}/thumb/{imageName}";
            }

            else if (!string.IsNullOrEmpty(Subscriber.ImageUrl))
            {
                model.ImageUrl = Subscriber.ImageUrl;
                model.ImageThumbnailUrl = Subscriber.ImageThumbnailUrl;
            }

            Subscriber = _mapper.Map(model, Subscriber);
            Subscriber.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            Subscriber.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = Subscriber.Id });
        }

        [AjaxOnly]
        public IActionResult GetAreas(int governorateId)
        {
            var areas = _context.Areas
                    .Where(a => a.GovernorateId == governorateId && !a.IsDeleted)
                    .OrderBy(g => g.Name)
                    .ToList();

            return Ok(_mapper.Map<IEnumerable<SelectListItem>>(areas));
        }

        public IActionResult AllowNationalId(SubscriberFormViewModel model)
        {
            var Subscriber = _context.Subscribers.SingleOrDefault(b => b.NationalId == model.NationalId);
            var isAllowed = Subscriber is null || Subscriber.Id.Equals(model.Id);

            return Json(isAllowed);
        }

        public IActionResult AllowMobileNumber(SubscriberFormViewModel model)
        {
            var Subscriber = _context.Subscribers.SingleOrDefault(b => b.MobileNumber == model.MobileNumber);
            var isAllowed = Subscriber is null || Subscriber.Id.Equals(model.Id);

            return Json(isAllowed);
        }

        public IActionResult AllowEmail(SubscriberFormViewModel model)
        {
            var Subscriber = _context.Subscribers.SingleOrDefault(b => b.Email == model.Email);
            var isAllowed = Subscriber is null || Subscriber.Id.Equals(model.Id);

            return Json(isAllowed);
        }

        private SubscriberFormViewModel PopulateViewModel(SubscriberFormViewModel? model = null)
        {
            SubscriberFormViewModel viewModel = model is null ? new SubscriberFormViewModel() : model;

            var governorates = _context.Governorates.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            viewModel.Governorates = _mapper.Map<IEnumerable<SelectListItem>>(governorates);
            
            if(model?.GovernorateId > 0)
            {
                var areas = _context.Areas
                    .Where(a => a.GovernorateId == model.GovernorateId && !a.IsDeleted)
                    .OrderBy(a => a.Name)
                    .ToList();

                viewModel.Areas = _mapper.Map<IEnumerable<SelectListItem>>(areas);
            }
            
            return viewModel;
        }
    }
}