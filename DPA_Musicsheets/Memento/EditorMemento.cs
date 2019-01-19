namespace DPA_Musicsheets.Memento
{
    class EditorMemento
    {
        public string EditorText { get; set; }

        public EditorMemento(string text)
        {
            EditorText = text;
        }
    }
}
