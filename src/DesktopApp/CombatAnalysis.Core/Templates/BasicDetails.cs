using CombatAnalysis.Core.Interfaces.Entities;
using CombatAnalysis.Core.ViewModels.Base;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.Templates;

public class BasicDetails : ParentTemplate<KeyValuePair<ObservableCollection<IDetailsEntity>, ObservableCollection<IGeneralDetailsEntity>>>
{
    private ObservableCollection<IDetailsEntity>? _detailsInformations;
    private ObservableCollection<IGeneralDetailsEntity>? _generalInformations;

    public ObservableCollection<IDetailsEntity>? DetailsInformations
    {
        get { return _detailsInformations; }
        set
        {
            SetProperty(ref _detailsInformations, value);
        }
    }

    public ObservableCollection<IGeneralDetailsEntity>? GeneralInformations
    {
        get { return _generalInformations; }
        set
        {
            SetProperty(ref _generalInformations, value);
        }
    }

    public override void Prepare(KeyValuePair<ObservableCollection<IDetailsEntity>, ObservableCollection<IGeneralDetailsEntity>> parameter)
    {
        DetailsInformations = parameter.Key;
        GeneralInformations = parameter.Value;
    }
}
