using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Forms;
using FK_CLI;
using FK_FormHelper;

namespace MaterialEditor
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		fk_Model _model = new fk_Model();
		fk_Solid _shape = new fk_Solid();

		fk_Model _camera = new fk_Model();

		bool eventBlock = false;

		fk_Vector oldPos = new fk_Vector();

		public MainWindow()
		{
			this.eventBlock = true;

			InitializeComponent();

			fk_Material.InitDefault();

			this.setupFK();

			this.setupGUI();

			this.eventBlock = false;

			return;
		}

		private void setupFK()
		{
			var viewport = new fk_Viewport(ViewportPanel);

			var scene = new fk_Scene();
			scene.BGColor = new fk_Color(0.8, 0.8, 0.8);

			viewport.Scene = scene;

			this._model.Shape = this._shape;
			this._model.Material = new fk_Material();

			scene.EntryModel(this._model);


			this._camera.GlTranslate(0.0, 0.0, 50.0);
			this._camera.GlFocus(0.0, 0.0, 0.0);

			scene.Camera = this._camera;


			var light = new fk_Light();
			var m_light = new fk_Model();
			light.Type = fk_LightType.PARALLEL;
			m_light.Shape = light;
			m_light.GlFocus(-1.0, -1.0, -1.0);
			m_light.Material = fk_Material.White;

			m_light.SetParent(this._camera);

			scene.EntryModel(m_light);

			return;
		}

		private void setupGUI()
		{
			this.setShape();

			this.eventBlock = true;

			fk_Material mat = this._model.Material;

			this.Alpha.Text = mat.Alpha.ToString("F1");

			fk_Color col;

			col = mat.Ambient;
			this.Ambient_R.Text = col.r.ToString("F1");
			this.Ambient_G.Text = col.g.ToString("F1");
			this.Ambient_B.Text = col.b.ToString("F1");

			col = mat.Diffuse;
			this.Diffuse_R.Text = col.r.ToString("F1");
			this.Diffuse_G.Text = col.g.ToString("F1");
			this.Diffuse_B.Text = col.b.ToString("F1");

			col = mat.Specular;
			this.Specular_R.Text = col.r.ToString("F1");
			this.Specular_G.Text = col.g.ToString("F1");
			this.Specular_B.Text = col.b.ToString("F1");

			col = mat.Emission;
			this.Emission_R.Text = col.r.ToString("F1");
			this.Emission_G.Text = col.g.ToString("F1");
			this.Emission_B.Text = col.b.ToString("F1");

			this.Shininess.Text = mat.Shininess.ToString("F1");

			this.eventBlock = false;

			return;
		}

		private void setupMaterial()
		{
			fk_Material mat = new fk_Material();

			mat.Alpha = this.boundary(float.Parse(this.Alpha.Text));
			mat.Shininess = this.boundary(float.Parse(this.Shininess.Text), 10.0f, 200.0f);

			fk_Color col;
			col = mat.Ambient;
			col.r = this.boundary(float.Parse(this.Ambient_R.Text));
			col.g = this.boundary(float.Parse(this.Ambient_G.Text));
			col.b = this.boundary(float.Parse(this.Ambient_B.Text));
			mat.Ambient = col;

			col = mat.Diffuse;
			col.r = this.boundary(float.Parse(this.Diffuse_R.Text));
			col.g = this.boundary(float.Parse(this.Diffuse_G.Text));
			col.b = this.boundary(float.Parse(this.Diffuse_B.Text));
			mat.Diffuse = col;

			col = mat.Specular;
			col.r = this.boundary(float.Parse(this.Specular_R.Text));
			col.g = this.boundary(float.Parse(this.Specular_G.Text));
			col.b = this.boundary(float.Parse(this.Specular_B.Text));
			mat.Specular = col;

			col = mat.Emission;
			col.r = this.boundary(float.Parse(this.Emission_R.Text));
			col.g = this.boundary(float.Parse(this.Emission_G.Text));
			col.b = this.boundary(float.Parse(this.Emission_B.Text));
			mat.Emission = col;

			this._model.Material = mat;

			return;
		}

		private float boundary(float value, float min = 0.0f, float max = 1.0f)
		{
			if (value < min)
				return min;
			if (value > max)
				return max;

			return value;
		}

		private void setShape()
		{
			ComboBoxItem bItem = this.comboBox.SelectedItem as ComboBoxItem;

			String comboValue = bItem.Content as String;
			if (comboValue == "球")
			{
				this._shape.MakeSphere(20, 10.0);
				this._model.SmoothMode = true;
			}
			else if (comboValue == "立方体")
			{
				this._shape.MakeBlock(15.0, 15.0, 15.0);
				this._model.SmoothMode = false;
			}
			else
			{
				Console.WriteLine("unknown");
			}

			return;
		}

		private void TextChanged(object sender, TextChangedEventArgs e)
		{
			if (this.eventBlock)
				return;

			System.Windows.Controls.TextBox tBox = sender as System.Windows.Controls.TextBox;

			float value;
			if (!float.TryParse(tBox.Text, out value))
			{
				return;
			}

			this.setupMaterial();

			return;
		}

		private void comboBox_preset_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem bItem = this.comboBox_preset.SelectedItem as ComboBoxItem;
			String comboValue = bItem.Content as String;

			fk_Material mat = null;
			if (comboValue == "AshGray")
			{
				mat = fk_Material.AshGray;
			}
			else if (comboValue == "BambooGreen")
			{
				mat = fk_Material.BambooGreen;
			}
			else if (comboValue == "Blue")
			{
				mat = fk_Material.Blue;
			}
			else if (comboValue == "Brown")
			{
				mat = fk_Material.Brown;
			}
			else if (comboValue == "BurntTitan")
			{
				mat = fk_Material.BurntTitan;
			}
			else if (comboValue == "Coral")
			{
				mat = fk_Material.Coral;
			}
			else if (comboValue == "Cream")
			{
				mat = fk_Material.Cream;
			}
			else if (comboValue == "Cyan")
			{
				mat = fk_Material.Cyan;
			}
			else if (comboValue == "DarkBlue")
			{
				mat = fk_Material.DarkBlue;
			}
			else if (comboValue == "DarkGreen")
			{
				mat = fk_Material.DarkGreen;
			}
			else if (comboValue == "DarkPurple")
			{
				mat = fk_Material.DarkPurple;
			}
			else if (comboValue == "DarkRed")
			{
				mat = fk_Material.DarkRed;
			}
			else if (comboValue == "DarkYellow")
			{
				mat = fk_Material.DarkYellow;
			}
			else if (comboValue == "DimYellow")
			{
				mat = fk_Material.DimYellow;
			}
			else if (comboValue == "Flesh")
			{
				mat = fk_Material.Flesh;
			}
			else if (comboValue == "GlossBlack")
			{
				mat = fk_Material.GlossBlack;
			}
			else if (comboValue == "GrassGreen")
			{
				mat = fk_Material.GrassGreen;
			}
			else if (comboValue == "Gray1")
			{
				mat = fk_Material.Gray1;
			}
			else if (comboValue == "Gray2")
			{
				mat = fk_Material.Gray2;
			}
			else if (comboValue == "Green")
			{
				mat = fk_Material.Green;
			}
			else if (comboValue == "HolidaySkyBlue")
			{
				mat = fk_Material.HolidaySkyBlue;
			}
			else if (comboValue == "IridescentGreen")
			{
				mat = fk_Material.IridescentGreen;
			}
			else if (comboValue == "Ivory")
			{
				mat = fk_Material.Ivory;
			}
			else if (comboValue == "LavaRed")
			{
				mat = fk_Material.LavaRed;
			}
			else if (comboValue == "LightBlue")
			{
				mat = fk_Material.LightBlue;
			}
			else if (comboValue == "LightCyan")
			{
				mat = fk_Material.LightCyan;
			}
			else if (comboValue == "LightGreen")
			{
				mat = fk_Material.LightGreen;
			}
			else if (comboValue == "LightViolet")
			{
				mat = fk_Material.LightViolet;
			}
			else if (comboValue == "Lilac")
			{
				mat = fk_Material.Lilac;
			}
			else if (comboValue == "MatBlack")
			{
				mat = fk_Material.MatBlack;
			}
			else if (comboValue == "Orange")
			{
				mat = fk_Material.Orange;
			}
			else if (comboValue == "PaleBlue")
			{
				mat = fk_Material.PaleBlue;
			}
			else if (comboValue == "PearWhite")
			{
				mat = fk_Material.PearWhite;
			}
			else if (comboValue == "Pink")
			{
				mat = fk_Material.Pink;
			}
			else if (comboValue == "Purple")
			{
				mat = fk_Material.Purple;
			}
			else if (comboValue == "Red")
			{
				mat = fk_Material.Red;
			}
			else if (comboValue == "TrueWhite")
			{
				mat = fk_Material.TrueWhite;
			}
			else if (comboValue == "UltraMarine")
			{
				mat = fk_Material.UltraMarine;
			}
			else if (comboValue == "Violet")
			{
				mat = fk_Material.Violet;
			}
			else if (comboValue == "White")
			{
				mat = fk_Material.White;
			}
			else if (comboValue == "Yellow")
			{
				mat = fk_Material.Yellow;
			}
			else
			{
				mat = new fk_Material();
			}

			this._model.Material = mat;

			this.setupGUI();

			return;
		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.setShape();

			return;
		}

		private void ViewportPanel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				fk_Vector oldPos = this.oldPos;
				fk_Vector newPos = new fk_Vector(e.X, e.Y, 0.0);

				fk_Vector diff = newPos - oldPos;

				this._camera.GlRotateWithVec(0.0, 0.0, 0.0, fk_Axis.Y, -diff.x * 0.01);
				this._camera.GlRotateWithVec(new fk_Vector(), this._camera.Upvec ^ this._camera.Vec, diff.y * 0.01);

				this.oldPos = newPos;
			}

			return;
		}

		private void ViewportPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left)
			{
				this.oldPos.Set(e.X, e.Y);
			}

			return;
		}

		private void Alpha_LostFocus(object sender, RoutedEventArgs e)
		{
			this.setupGUI();
			return;
		}
	}
}
