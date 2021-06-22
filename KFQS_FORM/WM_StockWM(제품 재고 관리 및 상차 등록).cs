using DC_POPUP;
using DC00_assm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KFQS_Form
{
    public partial class WM_StockWM : DC00_WinForm.BaseMDIChildForm
    {
        // 그리드를 세팅 할 수 있드록 도와주는 함수 클래스
        UltraGridUtil _GridUtil = new UltraGridUtil();
        // 공장 변수 입력
        private string sPlantCode = LoginInfo.UserID;

        public WM_StockWM()
        {
            InitializeComponent();
        }

        private void WM_StockWM_Load(object sender, EventArgs e)
        {
            // 그리드 세팅
            try
            {

                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "CHK",         "상차 등록",    true, GridColDataType_emu.CheckBox, 130, 130, Infragistics.Win.HAlign.Center, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE",   "공장",         true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "SHIPFLAG",    "상차여부",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE",    "품목",         true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME",    "품목명",       true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "LOTNO",       "LOTNO",        true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WHCODE",      "입고창고",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "STOCKQTY",    "재고수량",     true, GridColDataType_emu.Double,   130, 130, Infragistics.Win.HAlign.Right,  true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE",    "단위",         true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "INDATE",      "입고 일자",    true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE",    "등록 일시",    true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKER",       "등록자",       true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left,   true, false);

                // 그리드 바인딩
                _GridUtil.SetInitUltraGridBind(grid1);

                //콤보 박스 셋팅
                Common _Common = new Common();
                DataTable dtTemp = new DataTable();
                //PLANTCODE 기준정보 가져와서 데이터 테이블에 추가

                dtTemp = _Common.Standard_CODE("PLANTCODE");
                //데이터 테이블에 있는 데이터를 해당 콤보박스에 추가
                Common.FillComboboxMaster(this.cboPlantCode_H, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", dtTemp, "CODE_ID", "CODE_NAME");

                BizTextBoxManager btbManager1 = new BizTextBoxManager();
                btbManager1.PopUpAdd(txtItemCode, txtItemName, "ITEM_MASTER", new object[] { "1000", "", });

                BizTextBoxManager btbManager2 = new BizTextBoxManager();
                btbManager2.PopUpAdd(txtWorkerID, txtWorkerName, "WORKER_MASTER", new object[] { "", "","","","" });

                BizTextBoxManager btbManager3 = new BizTextBoxManager();
                btbManager3.PopUpAdd(txtCustID, txtCustName, "CUST_MASTER", new object[] { "", "", "" });

                dtTemp = _Common.Standard_CODE("YESNO");
                Common.FillComboboxMaster(this.cboShipFlag, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName,"ALL","");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "SHIPFLAG", dtTemp, "CODE_ID", "CODE_NAME");

                dtTemp = _Common.Standard_CODE("WHCODE");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "WHCODE", dtTemp, "CODE_ID", "CODE_NAME");

            }
            catch (Exception ex)
            {

                ShowDialog(ex.Message, DC00_WinForm.DialogForm.DialogType.OK);
            }
        }

        public override void DoInquire()
        {
            base.DoInquire();
            DBHelper helper = new DBHelper(false);
            try
            {
                string sPlantCode = Convert.ToString(cboPlantCode_H.Value);
                string sItemcode = Convert.ToString(txtItemCode.Text);
                string sShipFlag = Convert.ToString(cboShipFlag.Value);
                string sLotno     = Convert.ToString(txtLotNo.Text);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", dtpStart.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);
                
                

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("06WM_StockWM_S1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input)
                                                                                        , helper.CreateParameter("LOTNO"    , sLotno,     DbType.String, ParameterDirection.Input)
                                                                                        , helper.CreateParameter("ITEMCODE" , sItemcode,  DbType.String, ParameterDirection.Input)
                                                                                        , helper.CreateParameter("SHIPFLAG" , sShipFlag,  DbType.String, ParameterDirection.Input)
                                                                                        , helper.CreateParameter("STARTDATE", sStartDate, DbType.String, ParameterDirection.Input)
                                                                                        , helper.CreateParameter("ENDDATE"  , sEndDate,   DbType.String, ParameterDirection.Input));


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

            if (this.txtCarNum.Text == "")
            {
                this.ShowDialog("차량 번호를 선택하세요.", DC00_WinForm.DialogForm.DialogType.OK);
                return;
            }
            if (this.txtWorkerID.Text == "")
            {
                this.ShowDialog("작업자를 선택하세요.", DC00_WinForm.DialogForm.DialogType.OK);
                return;
            }
            if (this.txtCustID.Text == "")
            {
                this.ShowDialog("거래처명을 선택하세요.", DC00_WinForm.DialogForm.DialogType.OK);
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
                    
                    string sCarno = Convert.ToString(txtCarNum.Text);
                    string sCustName = Convert.ToString(txtCustID.Text);
                    string sWorkerName = Convert.ToString(txtWorkerID.Text);
                    string temp;
                    if (i == 0) temp = Convert.ToString(i);
                    else temp = helper.RSMSG;

                    if (Convert.ToString(dtTemp.Rows[i]["CHK"]) == "0") continue;

                    helper.ExecuteNoneQuery("06WM_StockWM_U1", CommandType.StoredProcedure //, helper.CreateParameter("FLAG", temp, DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("PLANTCODE", Convert.ToString(dtTemp.Rows[i]["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("LOTNO", Convert.ToString(dtTemp.Rows[i]["LOTNO"]), DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("ITEMCODE", Convert.ToString(dtTemp.Rows[i]["ITEMCODE"]), DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("CUSTCODE", sCustName, DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("CARNO", sCarno, DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("STOCKQTY", Convert.ToString(dtTemp.Rows[i]["STOCKQTY"]), DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("SHIPSEQ", temp , DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("WORKER", sWorkerName, DbType.String, ParameterDirection.Input)
                                                                                            , helper.CreateParameter("MAKER", Convert.ToString(dtTemp.Rows[i]["MAKER"]), DbType.String, ParameterDirection.Input));

                    if (helper.RSCODE == "E")
                    {
                        this.ShowDialog(helper.RSMSG, DC00_WinForm.DialogForm.DialogType.OK);
                        helper.Rollback();
                        return;
                    }

                    helper.Commit();
                    if (i == dtTemp.Rows.Count - 1)
                    {
                        this.ShowDialog(i + 1 + "개 데이터가 정상적으로 등록 되었습니다", DC00_WinForm.DialogForm.DialogType.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
                helper.Rollback();
            }
            finally
            {
                DoInquire();
                helper.Close();
            }
        }
    }
}
