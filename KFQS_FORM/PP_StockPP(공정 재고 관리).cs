using DC00_assm;
using DC_POPUP;
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
    public partial class PP_StockPP : DC00_WinForm.BaseMDIChildForm
    {
        DataTable rtnDtTemp = new DataTable(); // 
        UltraGridUtil _GridUtil = new UltraGridUtil();  //그리드 객체 생성
        Common _Common = new Common();
        string plantCode = LoginInfo.PlantCode;

        public PP_StockPP()
        {
            InitializeComponent();
        }

        private void PP_StockPP_Load(object sender, EventArgs e)
        {
                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "CHK",      "원자재 출고취소",true, GridColDataType_emu.CheckBox,    130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE","공장",           true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "입고 일자",      true, GridColDataType_emu.DateTime24,  130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목",           true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품목 명",        true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "LOTNO", "LOTNO",             true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMTYPE", "품목 구분",      true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "STOCKQTY", "수량",           true, GridColDataType_emu.Double,      130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE", "단위",           true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WHCODE", "입고 창고",        true, GridColDataType_emu.VarChar,     130, 130, Infragistics.Win.HAlign.Left, true, false);


                // 그리드 바인딩
            _GridUtil.SetInitUltraGridBind(grid1);

            Common _Common = new Common();
            DataTable rtnDtTemp = new DataTable();

            rtnDtTemp = _Common.Standard_CODE("PLANTCODE");  // 사업장
            Common.FillComboboxMaster(this.cboPlantCode_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.Standard_CODE("UNITCODE");     //단위
            UltraGridUtil.SetComboUltraGrid(this.grid1, "UNITCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.Standard_CODE("WHCODE");     //입고 창고
            UltraGridUtil.SetComboUltraGrid(this.grid1, "WHCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            rtnDtTemp = _Common.GET_ItemCodeFERT_Code("ITEMTYPE"); // 품목 구분
            Common.FillComboboxMaster(this.cboCust_H, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "ITEMTYPE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            cboPlantCode_H.Value = plantCode;

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
                dtTemp = helper.FillTable("06PP_StockPP_S1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE", sPlantcode, DbType.String, ParameterDirection.Input)
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

        public override void DoSave()
        {
            base.DoSave();
            DataTable dtTemp = new DataTable();
            dtTemp = grid1.chkChange();
            if (dtTemp == null) return;

            DBHelper helper = new DBHelper("", true);
            try
            {
                if (this.ShowDialog("원자재 생산 출고 취소를 하시겠습니까 ? ") == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (Convert.ToString(dtTemp.Rows[i]["CHK"]) == "0") continue;

                    if (Convert.ToString(dtTemp.Rows[i]["ITEMTYPE"]) != "ROH")
                    {
                        ShowDialog("원자재가 아닌 LOT 는 원자재 출고 취소를 할 수 없습니다.", DialogForm.DialogType.OK);
                        helper.Rollback();
                        return;
                    }
                    helper.ExecuteNoneQuery("06PP_StockPP_U1", CommandType.StoredProcedure
                                                , helper.CreateParameter("PLANTCODE", Convert.ToString(dtTemp.Rows[i]["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("LotNo", Convert.ToString(dtTemp.Rows[i]["LOTNO"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("ItemCode", Convert.ToString(dtTemp.Rows[i]["ITEMCODE"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("Qty", Convert.ToString(dtTemp.Rows[i]["STOCKQTY"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("UnitCode", Convert.ToString(dtTemp.Rows[i]["UNITCODE"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("WorkerId", this.WorkerID, DbType.String, ParameterDirection.Input));
                    

                    if (helper.RSCODE == "E")
                    {
                        this.ShowDialog(helper.RSMSG, DialogForm.DialogType.OK);
                        helper.Rollback();
                        return;
                    }

                    helper.Commit();
                    this.ShowDialog("정상적으로 등록 되었습니다", DC00_WinForm.DialogForm.DialogType.OK);
                    this.ClosePrgForm();
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
        
        //LOT 발행
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            if (grid1.ActiveRow == null) return;
            if (Convert.ToString(this.grid1.ActiveRow.Cells["ITEMTYPE"].Value) == "FERT")
            {
                DBHelper helper = new DBHelper();
                try
                {
                    string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                    string sLotNo = Convert.ToString(grid1.ActiveRow.Cells["LOTNO"].Value);

                    DataTable dtTemp = helper.FillTable("06PP_StockPP_S2", CommandType.StoredProcedure
                                                         , helper.CreateParameter("PLANTCODE"   , sPlantCode    , DbType.String, ParameterDirection.Input)
                                                         , helper.CreateParameter("LOTNO"       , sLotNo        , DbType.String, ParameterDirection.Input));
                    if (dtTemp.Rows.Count == 0)
                    {
                        ShowDialog("바코드 정보를 조회 할 내용이 없습니다.", DialogForm.DialogType.OK);
                        return;
                    }
                    // 바코드 디자인 선언
                    Report_LotBacodeFERT sReportFert = new Report_LotBacodeFERT();
                    // 바코드 디자인이 첨부될 레포트 북 클래스 선언
                    Telerik.Reporting.ReportBook repBook = new Telerik.Reporting.ReportBook();
                    //바코드 디자인에 데이터 바인딩
                    sReportFert.DataSource = dtTemp;
                    //레포트 북에 디자인 추가
                    repBook.Reports.Add(sReportFert);

                    //레포트 미리보기 창에 레포트 북 등록 및 출력 장수 입력
                    ReportViewer barcodeViewer = new ReportViewer(repBook, 1);
                    //미리보기 창 호출
                    barcodeViewer.ShowDialog();
                }
                catch (Exception ex)
                {
                    ShowDialog(ex.ToString());
                }
                finally
                {
                    helper.Close();
                }
            }
        }
    }
}
