using DC00_assm;
using DC00_WinForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KFQS_Form
{
    public partial class MM_StockOUT : DC00_WinForm.BaseMDIChildForm
    {
        DataTable rtnDtTemp = new DataTable(); // 
        UltraGridUtil _GridUtil = new UltraGridUtil();  //그리드 객체 생성
        Common _Common = new Common();
        string plantCode = LoginInfo.PlantCode;

        public MM_StockOUT()
        {
            InitializeComponent();
        }

        private void MM_StockOUT_Load(object sender, EventArgs e)
        {
                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "CHK",      "선택",     true, GridColDataType_emu.CheckBox,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1 , "PLANTCODE","공장",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "입고일자", true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품목명",   true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MATLOTNO", "LOTNO",   true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "STOCKQTY", "수량",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE", "단위",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WHCODE",   "창고",     true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKER",    "입고자",   true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, false);


                // 그리드 바인딩
            _GridUtil.SetInitUltraGridBind(grid1);

            Common _Common = new Common();
            DataTable rtnDtTemp = new DataTable();

            rtnDtTemp = _Common.Standard_CODE("PLANTCODE");  // 사업장
            Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_ItemCodeFERT_Code("ROH");
            Common.FillComboboxMaster(this.cboCust_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            rtnDtTemp = _Common.Standard_CODE("WHCODE", "MINORCODE = 'WH003'");
            Common.FillComboboxMaster(this.ultraComboEditor1, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");


            rtnDtTemp = _Common.Standard_CODE("STORAGELOCCODE","RELCODE1 = 'WH003'");
            Common.FillComboboxMaster(this.ultraComboEditor2, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            string sPlantCode = Convert.ToString(this.cboPlantCode_H.Value);
            this.cboPlantCode_H.Value = "1000";
        }

        public override void DoInquire()
        {
            base.DoInquire();
            DBHelper helper = new DBHelper(false);
            try
            {
                string sPlantcode = Convert.ToString(cboPlantCode_H.Value);
                string sItemcode  = cboCust_H.Value.ToString();
                string sMATLOTNO  = txtLOTNO.Text.ToString();
                string sStart     = string.Format("{0:yyyy-MM-dd}", dtpStart.Value);
                string sEnd       = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("06MM_StockOUT_S1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE",sPlantcode, DbType.String, ParameterDirection.Input)
                                                                                         , helper.CreateParameter("ITEMCODE", sItemcode,  DbType.String, ParameterDirection.Input)
                                                                                         , helper.CreateParameter("MATLOTNO", sMATLOTNO,  DbType.String, ParameterDirection.Input)
                                                                                         , helper.CreateParameter("STARTDATE",sStart,     DbType.String, ParameterDirection.Input)
                                                                                         , helper.CreateParameter("ENDDATE",  sEnd ,      DbType.String, ParameterDirection.Input));

                this.ClosePrgForm();
                if (dtTemp.Rows.Count > 0)
                {
                    grid1.DataSource = dtTemp;
                    grid1.DataBinds(dtTemp);
                }
                else 
                {
                    _GridUtil.Grid_Clear(grid1);
                    ShowDialog("조회할 데이터가 없습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                }
            }
            catch (Exception ex)
            {

                ShowDialog(ex.Message, DC00_WinForm.DialogForm.DialogType.OK);
            }
            finally
            {
                helper.Close();
            }
        }



        public override void DoNew()
        {

        }
        public override void DoDelete()
        {

        }

        public override void DoSave()
        {
            base.DoSave();
            DataTable dtTemp = new DataTable();
            dtTemp = grid1.chkChange();
            if (dtTemp == null) return;

            if (this.ultraComboEditor1.Value.ToString() == "")
            {
                this.ShowDialog("창고를 선택하세요", DialogForm.DialogType.OK);
                return;
            }
                DBHelper helper = new DBHelper("", true);
            try
            {
                if (ShowDialog("해당 사항을 저장 하시겠습니까?", DC00_WinForm.DialogForm.DialogType.YESNO) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (Convert.ToString(dtTemp.Rows[i]["CHK"]) == "0") continue;


                    helper.ExecuteNoneQuery("06MM_StockOUT_U1", CommandType.StoredProcedure
                                                , helper.CreateParameter("PLANTCODE",      Convert.ToString(dtTemp.Rows[i]["PLANTCODE"]),  DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("MATLOTNO",       Convert.ToString(dtTemp.Rows[i]["MATLOTNO"]),DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("ITEMCODE",       Convert.ToString(dtTemp.Rows[i]["ITEMCODE"]),   DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("QTY",            Convert.ToString(dtTemp.Rows[i]["STOCKQTY"]),   DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("UNITCODE",       Convert.ToString(dtTemp.Rows[i]["UNITCODE"]),   DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("WHCODE",         Convert.ToString(ultraComboEditor1.Value),      DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("STORAGELOCCODE", Convert.ToString(ultraComboEditor2.Value),      DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("WORKERID",       this.WorkerID,                                  DbType.String, ParameterDirection.Input));

                    if (helper.RSCODE == "E")
                    {
                        this.ShowDialog(helper.RSMSG, DialogForm.DialogType.OK);
                        helper.Rollback();
                        return;
                    }

                        helper.Commit();
                        this.ShowDialog("정상적으로 등록 되었습니다", DC00_WinForm.DialogForm.DialogType.OK);
                        DoInquire();
                }
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
                helper.Rollback();
            }
            finally
            {
                helper.Close();
            }
        }
    }
}
