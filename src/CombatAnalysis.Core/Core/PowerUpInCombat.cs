using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.Core
{
    public class PowerUpInCombat<T>
        where T : class
    {
        private string _propertyName = string.Empty;
        private ObservableCollection<T> _dependencyCollection;

        public PowerUpInCombat(ObservableCollection<T> dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        public void UpdateCollection(ObservableCollection<T> dependencyCollection)
        {
            _dependencyCollection = dependencyCollection;
        }

        public void UpdateProperty(string propertyName)
        {
            _propertyName = propertyName;
        }

        public ObservableCollection<T> ShowSpecificalValue(string sortPropertyName, ObservableCollection<T> targetCollection, bool isShowValue)
        {
            var type = typeof(T);
            var property = type.GetProperty(_propertyName);

            if (!isShowValue)
            {
                foreach (var item in _dependencyCollection)
                {
                    if ((bool)property.GetValue(item))
                    {
                        targetCollection.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in _dependencyCollection)
                {
                    if ((bool)property.GetValue(item))
                    {
                        targetCollection.Add(item);
                    }
                }

                Sorts<T>.SetProperty(sortPropertyName);
                targetCollection = Sorts<T>.BubbleSort(targetCollection);
            }

            return targetCollection;
        }

        public ObservableCollection<T> ShowSpecificalValueInversion(string sortPropertyName, ObservableCollection<T> targetCollection, bool isShowValue)
        {
            var type = typeof(T);
            var property = type.GetProperty(_propertyName);

            if (isShowValue)
            {
                foreach (var item in _dependencyCollection)
                {
                    if (!(bool)property.GetValue(item))
                    {
                        targetCollection.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in _dependencyCollection)
                {
                    if (!(bool)property.GetValue(item))
                    {
                        targetCollection.Add(item);
                    }
                }

                Sorts<T>.SetProperty(sortPropertyName);
                targetCollection = Sorts<T>.BubbleSort(targetCollection);
            }

            return targetCollection;
        }
    }
}
