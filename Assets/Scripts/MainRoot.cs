using strange.extensions.context.impl;

public class MainRoot : ContextView
{
    private void Awake()
    {
        context = new MainContext(this);
    }
}
