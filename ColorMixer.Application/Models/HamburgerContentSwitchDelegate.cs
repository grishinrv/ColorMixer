using MahApps.Metro.Controls;

namespace ColorMixer.Application.Models
{
    /// <summary>
    /// When main hamburger menu's content is going to switch
    /// </summary>
    /// <param name="newContentView">A new view to show</param>
    /// <param name="closingContentView">Previous shown view</param>
    public delegate void HamburgerContentSwitchDelegate(HamburgerMenuItem? newContentView, HamburgerMenuItem? closingContentView);
}
