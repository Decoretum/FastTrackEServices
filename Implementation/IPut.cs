using Microsoft.AspNetCore.Mvc;

namespace FastTrackEServices.Implementation;

interface IPut {
    ICollection<IActionResult> put();
}