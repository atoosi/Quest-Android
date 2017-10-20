using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;
using System.Drawing;

namespace iso_demo
{
  public partial class TextFieldCell : UITableViewCell
	{
    public UITextField textField;
    public UILabel label;
    public string name;

    public TextFieldCell(string cellId) : base (UITableViewCellStyle.Default, cellId)
    {
      textField = new UITextField ();
      label = new UILabel ();
      ContentView.Add (label);
      ContentView.Add (textField);
      textField.ShouldReturn += (textFieldE) => { 
        textFieldE.ResignFirstResponder(); 
        return true;
      };
    }

    public void UpdateCell (string text, string value)
    {
      name = text;
      label.Text = text + ":";
      if (value != null) textField.Text = value;
      textField.AutocorrectionType = UITextAutocorrectionType.No;
      textField.KeyboardAppearance = UIKeyboardAppearance.Light;
      textField.KeyboardType = UIKeyboardType.Default;
    }

    public override void LayoutSubviews ()
    {
      base.LayoutSubviews ();
      int textFieldWidth = (int)ContentView.Bounds.Width - 100;
      int lableFieldWidth = (int)ContentView.Bounds.Width - textFieldWidth;
      textField.Frame = new Rectangle(120, 10, textFieldWidth, 25);
      label.Frame = new Rectangle(20, 10, lableFieldWidth, 25);
    }

    public string GetText() {
      return textField.Text;
    }

    public string GetName() {
      return name;
    }
  }
}