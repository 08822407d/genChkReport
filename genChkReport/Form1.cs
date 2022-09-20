using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Utils;

namespace genChkReport
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();

			//tbx_inputDIR.Text = "‪C:\\Users\\cheyh\\Desktop\\testdata\\5米DEM检查记录\\检查记录";
			//tbx_outputDIR.Text = "C:\\Users\\cheyh\\Desktop\\testdata\\5米DEM检查记录\\输出报告";
		}

		private void btn_inputDIR_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd =new FolderBrowserDialog();
			fbd.Description = "请选择文件夹";
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				string dir = fbd.SelectedPath;
				if (dir != null)
				{
					tbx_inputDIR.Text = dir;
				}
			}
		}

		private void btn_outputDIR_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd =new FolderBrowserDialog();
			fbd.Description = "请选择文件夹";
			if (fbd.ShowDialog() == DialogResult.OK)
			{
				string dir = fbd.SelectedPath;
				if (dir != null)
				{
					tbx_outputDIR.Text = dir;
				}
			}
		}

		private void btn_cancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btn_start_Click(object sender, EventArgs e)
		{
			string input_dir = tbx_inputDIR.Text;
			string output_dir = tbx_outputDIR.Text;

			if (!Directory.Exists(input_dir))
			{
				MessageBox.Show("错误：输入文件夹不存在");
				return;
			}
			if (!Directory.Exists(output_dir))
			{
				MessageBox.Show("错误：输出文件夹不存在");
				return;
			}

			List<string> excel_files = Utils.FIO.traverseSearchFile_Ext(input_dir, "xls");
			excel_files = excel_files.Union(Utils.FIO.traverseSearchFile_Ext(input_dir, "xlsx")).ToList<string>();
		}
	}
}
