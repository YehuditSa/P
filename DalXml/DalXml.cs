
using DalApi;
using DO;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;


sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public ITask Task => new TaskImplementation();

    public void Reset()
    {
        Engineer.ResetAll();
        Dependency.ResetAll();
        Task.ResetAll();
    }

    public DateTime? StartProjectDate
    {
        get
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            return root.Element("Dates")?.ToDateTimeNullable("StartProjectDate");
        }
        set
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            root.Descendants("StartProjectDate").First().SetValue(value ?? throw new DalNullException("Project Start Date can't be null"));
            XMLTools.SaveListToXMLElement(root, "data-config");

        }
    }

    public DateTime? EndProjectDate
    {
        get
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            return root.Element("Dates")?.ToDateTimeNullable("EndProjectDate");
        }
        set
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            root.Descendants("EndProjectDate").First().SetValue(value ?? throw new DalNullException("Project End Date can't be null"));
            XMLTools.SaveListToXMLElement(root, "data-config");

        }
    }


}
