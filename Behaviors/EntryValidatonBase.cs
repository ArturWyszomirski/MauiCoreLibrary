namespace MauiCoreLibrary.Behaviors;

public abstract class EntryValidatonBase : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    protected abstract void OnEntryTextChanged(object sender, TextChangedEventArgs args);
}
