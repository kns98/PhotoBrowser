using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace PhotoAlbum
{
    public class SearchViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> canEnterSearchText;

        private readonly ObservableAsPropertyHelper<bool> isLoading;

        private ReactiveList<SearchResultViewModel> images;

        private string searchText;

        private bool showError;

        public SearchViewModel(IImageFinder imageFinder)
        {
            Images = new ReactiveList<SearchResultViewModel>();

            var canExecute = this.WhenAnyValue(x => x.SearchText)
                .Select(x => !string.IsNullOrWhiteSpace(x));

            Search = ReactiveCommand.CreateAsyncObservable(
                canExecute,
                _ =>
                {
                    Images.Clear();
                    ShowError = false;
                    return imageFinder.GetImages(SearchText);
                });

            Search.Subscribe(images => Images.Add(images));

            Search.ThrownExceptions.Subscribe(_ => ShowError = true);

            isLoading = Search.IsExecuting.ToProperty(this, vm => vm.IsLoading);

            canEnterSearchText = this.WhenAnyValue(x => x.IsLoading)
                .Select(x => !x)
                .ToProperty(this, vm => vm.CanEnterSearchText);
        }

        public ReactiveCommand<SearchResultViewModel> Search { get; set; }

        public string SearchText
        {
            get => searchText;
            set => this.RaiseAndSetIfChanged(ref searchText, value);
        }

        public bool ShowError
        {
            get => showError;
            set => this.RaiseAndSetIfChanged(ref showError, value);
        }

        public bool IsLoading => isLoading.Value;

        public bool CanEnterSearchText => canEnterSearchText.Value;

        public ReactiveList<SearchResultViewModel> Images
        {
            get => images;
            set => this.RaiseAndSetIfChanged(ref images, value);
        }
    }
}