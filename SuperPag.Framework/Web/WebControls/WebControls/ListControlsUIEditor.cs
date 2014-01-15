using System;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace SuperPag.Framework.Web.WebControls
{	
	/// <summary>
	/// Provides a user interface for selecting a state property.
	/// </summary>
	public class ListControlsUIEditor : UITypeEditor
	{
		// This class is the list that will pop up when we click
		// on the down arrow of the mock-combo box in the designer.
		#region PropertyList
		private class PropertiesList : ListBox
		{
			private IWindowsFormsEditorService edSvc;

			public PropertiesList(ComponentCollection components, IWindowsFormsEditorService service )
			{
				edSvc = service;
				string[] arrContainerButtons = GetIDButtons( components );
				this.PrepareListBox( arrContainerButtons );
			}

			private void PrepareListBox( string[] components ) 
			{
				base.IntegralHeight = true;
				if ( base.ItemHeight > 0 ) 
				{
					if ( components != null &&
						((base.Height / base.ItemHeight) < components.Length) ) 
					{
						//try to keep the listbox small but sufficient
						int adjHei= components.Length * base.ItemHeight;
						if ( adjHei > 200 ) adjHei = 200;
						base.Height = adjHei;
					}
				} 
				else //safeguard, although it shouldn't happen
					base.ItemHeight = 200;

				base.BorderStyle = BorderStyle.None;
				base.Sorted = true; //present in alphabetical order
				FillListBoxFromCollection( this, components);
				base.SelectedIndexChanged +=new EventHandler(handleSelection);
			}

			private void handleSelection(object sender, EventArgs e ) 
			{
				if ( this.edSvc != null ) 
				{
					edSvc.CloseDropDown();
				}
			}

			private static void FillListBoxFromCollection( ListBox lb, string[] components ) 
			{
				lb.BeginUpdate();
				lb.Items.Clear();
				foreach( string item in components ) 
					lb.Items.Add( item );
				lb.EndUpdate();
				lb.Invalidate();
			}

			private static string[] GetIDButtons( ComponentCollection  components ) 
			{
				System.Collections.ArrayList _list = new System.Collections.ArrayList();

				foreach( object item in components ) 
				{
					if ( item is System.Web.UI.WebControls.Button ) 
						_list.Add( ((System.Web.UI.WebControls.Button)item).ID );
				}

				return (string[])_list.ToArray(typeof(string));
			}
		}
		#endregion

		/// <summary>
		/// Displays a list of available values for the specified component than sets the value.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
		/// <param name="provider">A service provider object through which editing services may be obtained.</param>
		/// <param name="value">An instance of the value being edited.</param>
		/// <returns>The new value of the object. If the value of the object hasn't changed, this method should return the same object it was passed.</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				// This service is in charge of popping our ListBox.
				IWindowsFormsEditorService service1 = ((IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService)));

				if (service1 != null)
				{
					PropertiesList list = new PropertiesList( context.Container.Components , service1);

					// Drop the list control.
					service1.DropDownControl(list);

					// if an item's been selected, set it as value.
					if (list.SelectedIndex != -1)
						value = list.SelectedItem;
				}
			}

			return value;
		}

		/// <summary>
		/// Gets the editing style of the <see cref="EditValue"/> method.
		/// </summary>
		/// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
		/// <returns>Returns the DropDown style, since this editor uses a drop down list.</returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			// We're using a drop down style UITypeEditor.
			return UITypeEditorEditStyle.DropDown;
		}
	}
}
