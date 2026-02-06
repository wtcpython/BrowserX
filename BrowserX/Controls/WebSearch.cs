using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace BrowserX.Controls;

public sealed partial class WebSearch : ContentControl
{
    private AutoSuggestBox? _autoSuggestBox;

    public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs>? QuerySubmitted;

    public WebSearch()
    {
        DefaultStyleKey = typeof(WebSearch);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _autoSuggestBox = GetTemplateChild("AutoSuggestBox") as AutoSuggestBox;
        _autoSuggestBox!.TextChanged += AutoSuggestBox_TextChanged;
        _autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
        _autoSuggestBox.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
    }

    public string Text
    {
        get => _autoSuggestBox!.Text;
        set => _autoSuggestBox!.Text = value;
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            var text = sender.Text;
            var lastWord = text.Split(' ')[^1].ToLower();
            if (!string.IsNullOrEmpty(lastWord))
            {
                // sender.ItemsSource = App.searchEngine.SearchWords(lastWord).ToList();
            }
        }
    }

    private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        var text = sender.Text;
        var item = args.SelectedItem as string;

        var lastIndex = text.LastIndexOf(' ');
        if (lastIndex == -1)
        {
            sender.Text = item;
            return;
        }

        var prefix = text[..lastIndex];
        sender.Text = $"{prefix} {item}";
    }

    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion == null)
        {
            QuerySubmitted?.Invoke(sender, args);
        }
    }
}