using System.Drawing;

public class OrderHasColor
{
    public string OrderName { get; set; }
    public Color Color { get; set; }

    public OrderHasColor(string orderName, Color color)
    {
        OrderName = orderName;
        Color = color;
    }
}