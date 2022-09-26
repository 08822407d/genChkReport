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

using NPOI.XWPF.UserModel;

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

			this.Enabled = false;
			List<string> excel_files = Utils.FIO.traverseSearchFile_Ext(input_dir, "txt");
			excel_files = excel_files.Union(Utils.FIO.traverseSearchFile_Ext(input_dir, "csv")).ToList<string>();

			foreach (string f in excel_files)
				genReport(f, output_dir);

			this.Enabled = true;
			MessageBox.Show("处理完成");
		}

		void genReport(string f, string odir)
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
			lines[0] = lines[0].Replace("\t", "");
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
				//string chkrcd_1 = fields[chkrcd_1_idx];
				string handler_1 = fields[handler_1_idx];
				string handletime_1 = fields[handletime_1_idx];
				string rechecker_1 = fields[rechecker_1_idx];

				string checker_2 = fields[checker_2_idx];
				string chktime_2 = fields[chktime_2_idx];
				//string chkrcd_2 = fields[chkrcd_2_idx];
				string handler_2 = fields[handler_2_idx];
				string handletime_2 = fields[handletime_2_idx];
				string rechecker_2 = fields[rechecker_2_idx];

				string mark = fields[mark_idx];
				string rank = fields[rank_idx];

				if (mapno_idx == -1 ||
					producer_idx == -1 ||
					checker_1_idx == -1 ||
					chktime_1_idx == -1 ||
					handler_1_idx == -1 ||
					handletime_1_idx == -1 ||
					rechecker_1_idx == -1 ||
					checker_2_idx == -1 ||
					chktime_2_idx == -1 ||
					handler_2_idx == -1 ||
					handletime_2_idx == -1 ||
					rechecker_2_idx == -1 ||
					mark_idx == -1 ||
					rank_idx == -1)
					continue;

				string[] records1 = fields[chkrcd_1_idx].Split(';');
				string[] records2 = fields[chkrcd_2_idx].Split(';');

				string ofname_1 = Path.Combine(odir, mapno + "一级检查记录.docx");
				string ofname_2 = Path.Combine(odir, mapno + "二级检查记录.docx");

				// creat level1 report
				FileStream fs1 = new FileStream("tmplete1.docx", FileMode.Open, FileAccess.Read);
				XWPFDocument doc1 = new XWPFDocument(fs1);
				var tbl1 = doc1.Tables[0];

				tbl1.Rows[1].GetCell(1).SetText(mapno);
				tbl1.Rows[1].GetCell(3).SetText(producer);
				for (int i = 0; i < records1.Length; i++)
				{
					tbl1.Rows[i + 3].GetCell(0).SetText((i + 1).ToString());
					tbl1.Rows[i + 3].GetCell(1).SetText(records1[i]);
					tbl1.Rows[i + 3].GetCell(2).SetText("修改");
					tbl1.Rows[i + 3].GetCell(3).SetText("已改");
				}
				tbl1.Rows[21].GetCell(1).SetText(checker_1 + "  " + chktime_1);
				tbl1.Rows[21].GetCell(3).SetText(handler_1 + "\n" + handletime_1);
				tbl1.Rows[22].GetCell(0).RemoveParagraph(0);
				tbl1.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("复查者结论及质量：");
				tbl1.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("\t\t经复查，上述问题均已整改完毕，数据质量符合要求。");
				tbl1.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("");
				tbl1.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("复查者：" + rechecker_1);
				tbl1.Rows[22].GetCell(0).GetParagraphArray(3).Alignment = ParagraphAlignment.RIGHT;

				FileStream output1 = new FileStream(ofname_1, FileMode.Create);
				doc1.Write(output1);
				output1.Flush();
				output1.Close();
				fs1.Close();

				// creat level1 report
				FileStream fs2 = new FileStream("tmplete2.docx", FileMode.Open, FileAccess.Read);
				XWPFDocument doc2 = new XWPFDocument(fs2);
				var tbl2 = doc2.Tables[0];

				tbl2.Rows[1].GetCell(1).SetText(mapno);
				tbl2.Rows[1].GetCell(3).SetText(producer);
				for (int i = 0; i < records2.Length; i++)
				{
					tbl2.Rows[i + 3].GetCell(0).SetText((i + 1).ToString());
					tbl2.Rows[i + 3].GetCell(1).SetText(records2[i]);
					tbl2.Rows[i + 3].GetCell(2).SetText("修改");
					tbl2.Rows[i + 3].GetCell(3).SetText("已改");
				}
				tbl2.Rows[21].GetCell(1).SetText(checker_2 + "  " + chktime_2);
				tbl2.Rows[21].GetCell(3).SetText(handler_2 + "\n" + handletime_2);
				tbl2.Rows[22].GetCell(0).RemoveParagraph(0);
				tbl2.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("复查者结论及质量：");
				tbl2.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("\t\t经复查，上述问题均已整改完毕，整改后成果得分为" +
								mark.ToString() + "分，质量评定为“" + rank.ToString() + "”");
				tbl2.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("");
				tbl2.Rows[22].GetCell(0).AddParagraph().CreateRun().SetText("复查者：" + rechecker_2);
				tbl2.Rows[22].GetCell(0).GetParagraphArray(3).Alignment = ParagraphAlignment.RIGHT;

				FileStream output2 = new FileStream(ofname_2, FileMode.Create);
				doc2.Write(output2);
				output2.Flush();
				output2.Close();
				fs2.Close();
			}
		}
	}
}
