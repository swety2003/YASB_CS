// See https://aka.ms/new-console-template for more information


using WindowsDesktop;

VirtualDesktop.Configure();
var desktops = VirtualDesktop.GetDesktops();
var VirtualDesktops = desktops.ToList();

VirtualDesktop.CurrentChanged += (sender, eventArgs) =>
{
    Console.WriteLine("CurrentChanged");
};
VirtualDesktop.Created += (sender, desktop) =>
{
    Console.WriteLine("Created");

};

Console.WriteLine(VirtualDesktops);

while (true)
{
    
}