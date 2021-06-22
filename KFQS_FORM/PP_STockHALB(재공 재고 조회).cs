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
    public partial class PP_STockHALB : DC00_WinForm.BaseMDIChildForm
    {
        DataTable rtnDtTemp = new DataTable(); // 
        UltraGridUtil _GridUtil = new UltraGridUtil();  //그리드 객체 생성
        Common _Common = new Common();
        string plantCode = LoginInfo.PlantCode;

        public PP_STockHALB()
        {
            InitializeComponent();
        }

        private void PP_STockHALB_Load(object sender, EventArgs e)
        {
                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE"        ,"공장"             , true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE"         , "품목"            , true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME"         , "품목 명"         , true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMTYPE"         , "품목 구분"       , true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "LOTNO"            , "LOTNO"           , true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE"   , "작업장"          , true, GridColDataType_emu.Double,      130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME"   , "작업장명"        , true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "STOQTY"           , "재고수량"        , true, GridColDataType_emu.Double,      130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE"         , "단위"            , true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);


            // 그리드 바인딩
            _GridUtil.SetInitUltraGridBind(grid1);

            Common _Common = new Common();
            DataTable rtnDtTemp = new DataTable();

            rtnDtTemp = _Common.Standard_CODE("PLANTCODE");  
            Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_ItemCodeFERT_Code("ITEMTYPE"); // 품목 구분
            Common.FillComboboxMaster(this.cboCust_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "ITEMTYPE", rtnDtTemp, "CODE_ID", "CODE_NAME");

        }

        public override void DoInquire()
        {
            base.DoInquire();
            DBHelper helper = new DBHelper(false);
            try
            {
                string sPlantcode = cboPlantCode_H.Value.ToString();
                string sItemtype  = cboCust_H.Value.ToString();
                string sLotno  = txtLOTNO.Text.ToString();


                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("06PP_STockHALB_S1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE", sPlantcode, DbType.String, ParameterDirection.Input)
                                                                                          , helper.CreateParameter("ITEMTYPE", sItemtype, DbType.String, ParameterDirection.Input)
                                                                                          , helper.CreateParameter("LOTNO", sLotno, DbType.String, ParameterDirection.Input));
;

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
    }
}
