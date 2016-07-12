namespace ReportUnit.Model
{
    public class SideNavLink
    {
        public string FileName { get; private set; }

        public SideNavLink(string name)
        {
            FileName = name;
        }
    }
}
