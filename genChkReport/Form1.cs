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

			//tbx_inputDIR.Text = @"‪C:\Users\cheyh\Desktop\testdata\DemChkRcd\record";
			//tbx_outputDIR.Text = @"C:\Users\cheyh\Desktop\testdata\DemChkRcd\report";
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

			List<string> excel_files = Utils.FIO.traverseSearchFile_Ext(input_dir, "txt");
			//excel_files = excel_files.Union(Utils.FIO.traverseSearchFile_Ext(input_dir, "xlsx")).ToList<string>();

			foreach (string f in excel_files)
				getExcelData(f);
		}

		void getExcelData(string f)
		{
			string[] lines = File.ReadAllLines(f,Encoding.GetEncoding("gb2312"));
			if (lines.Length <= 1)
			{
				return;
			}

			int mapno_idx = -1;
			int producer_idx = -1;

			int checker_1_idx = -1;
			int chktime_1_idx = -1;
			int chkrcd_1_idx = -1;
			int handler_1_idx = -1;
			int handletime_1_idx = -1;
			int rechecker_1_idx = -1;

			int checker_2_idx = -1;
			int chktime_2_idx = -1;
			int chkrcd_2_idx = -1;
			int handler_2_idx = -1;
			int handletime_2_idx = -1;
			int rechecker_2_idx = -1;

			int mark_idx = -1;
			int rank_idx = -1;

			lines[0] = lines[0].Replace(" ", "");
			string[] header_fields = lines[0].Split(',');
			for (int i = 0; i < header_fields.Length; i++)
			{
				if (header_fields[i].Equals("图号"))
					mapno_idx = i;
				if (header_fields[i].Equals("作业员"))
					producer_idx = i;

				if (header_fields[i].Equals("一查检查员"))
					checker_1_idx = i;
				if (header_fields[i].Equals("一查时间"))
					chktime_1_idx = i;
				if (header_fields[i].Equals("一查处理者"))
					handler_1_idx = i;
				if (header_fields[i].Equals("一查处理时间"))
					handletime_1_idx = i;
				if (header_fields[i].Equals("一查意见"))
					chkrcd_1_idx = i;
				if (header_fields[i].Equals("一查复查者"))
					rechecker_1_idx = i;

				if (header_fields[i].Equals("二查检查员"))
					checker_2_idx = i;
				if (header_fields[i].Equals("二查时间"))
					chktime_2_idx = i;
				if (header_fields[i].Equals("二查处理者"))
					handler_2_idx = i;
				if (header_fields[i].Equals("二查处理时间"))
					handletime_2_idx = i;
				if (header_fields[i].Equals("二查意见"))
					chkrcd_2_idx = i;
				if (header_fields[i].Equals("二查复查者"))
					rechecker_2_idx = i;

				if (header_fields[i].Equals("二查得分"))
					mark_idx = i;
				if (header_fields[i].Equals("二查结论"))
					rank_idx = i;
			}

			for (int j = 1; j < lines.Length; j++)
			{
				string[] fields = lines[j].Replace(" ", "").Split(',');

				string mapno = fields[mapno_idx];
				string producer = fields[producer_idx];

				string checker_1 = fields[checker_1_idx];
				string chktime_1 = fields[chktime_1_idx];
				string chkrcd_1 = fields[chkrcd_1_idx];
				string handler_1 = fields[handler_1_idx];
				string handletime_1 = fields[handletime_1_idx];
				string rechecker_1 = fields[rechecker_1_idx];

				string checker_2 = fields[checker_2_idx];
				string chktime_2 = fields[chktime_2_idx];
				string chkrcd_2 = fields[chkrcd_2_idx];
				string handler_2 = fields[handler_2_idx];
				string handletime_2 = fields[handletime_2_idx];
				string rechecker_2 = fields[rechecker_2_idx];

				string mark = fields[mark_idx];
				string rank = fields[rank_idx];
			}
		}
	}
}
