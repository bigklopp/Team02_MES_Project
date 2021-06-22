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
    public partial class MM_StockMM : DC00_WinForm.BaseMDIChildForm
    {
        // 그리드를 세팅 할 수 있드록 도와주는 함수 클래스
        UltraGridUtil _GridUtil = new UltraGridUtil();
        // 공장 변수 입력
        private string sPlantCode = LoginInfo.UserID;

        public MM_StockMM()
        {
            InitializeComponent();
        }

        private void MM_StockMM_Load(object sender, EventArgs e)
        {
            // 그리드 세팅
            try
            {
                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE",  "공장",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE",   "품목",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME",   "품목명",   true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "MATLOTNO",   "LOT 번호", true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "WHCODE",     "창고",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "STOCKQTY",   "재고수량", true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE",   "단위",     true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "CUSTCODE",   "거래처",   true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "CUSTNAME",   "거래처명", true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKER",      "생성자",   true, GridColDataType_emu.VarChar,  130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE",   "생성일시", true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
               

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

                dtTemp = _Common.Standard_CODE("UNITCODE");     //단위
                UltraGridUtil.SetComboUltraGrid(this.grid1, "UNITCODE", dtTemp, "CODE_ID", "CODE_NAME");

                //부서 콤보 그리드뷰
                dtTemp = _Common.Standard_CODE("WHCODE");
                UltraGridUtil.SetComboUltraGrid(this.grid1, "WHCODE", dtTemp, "CODE_ID", "CODE_NAME");

                cboPlantCode_H.Value = LoginInfo.PlantCode;
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
                string sPlantcode = cboPlantCode_H.Value.ToString();
                string sItemCode  = txt_ItemCode.Text.ToString();
                string sItemName  = txt_ItemName.Text.ToString();

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("06MM_StockMM_S1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE", sPlantcode, DbType.String, ParameterDirection.Input)
                                                                                         , helper.CreateParameter("ITEMCODE", sItemCode,  DbType.String, ParameterDirection.Input)
                                                                                         , helper.CreateParameter("ITEMNAME", sItemName,  DbType.String, ParameterDirection.Input));

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
            base.DoNew();
            this.grid1.InsertRow();

            this.grid1.ActiveRow.Cells["PLANTCODE"].Value = LoginInfo.PlantCode;


            this.grid1.ActiveRow.Cells["PONO"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grid1.ActiveRow.Cells["CHK"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grid1.ActiveRow.Cells["LOTNO"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grid1.ActiveRow.Cells["INDATE"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grid1.ActiveRow.Cells["INWORKER"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

            this.grid1.ActiveRow.Cells["MAKER"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grid1.ActiveRow.Cells["MAKEDATE"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grid1.ActiveRow.Cells["EDITDATE"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            this.grid1.ActiveRow.Cells["EDITOR"].Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

        }
        public override void DoDelete()
        {
            base.DoDelete();
            if (Convert.ToString(this.grid1.ActiveRow.Cells["CHK"].Value) == "1")
            {
                ShowDialog("입고된 발주 내역은 삭제 할 수 없습니다",DC00_WinForm.DialogForm.DialogType.OK);
                return;
            }
            this.grid1.DeleteRow();
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
                if (ShowDialog("해당 사항을 저장 하시겠습니까?",DC00_WinForm.DialogForm.DialogType.YESNO) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                foreach (DataRow drrow in dtTemp.Rows)
                {
                    switch (drrow.RowState)
                    {
                        case DataRowState.Deleted:
                            drrow.RejectChanges();
                            helper.ExecuteNoneQuery("06MM_StockMM_D1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE", Convert.ToString(drrow["PLANTCODE"]), DbType.String, ParameterDirection.Input),
                                                                                                   helper.CreateParameter("PONO",      Convert.ToString(drrow["PONO"]), DbType.String, ParameterDirection.Input));
                            break;
                        case DataRowState.Added:
                            string sErrorMsg = string.Empty;
                            if (Convert.ToString(drrow["ITEMCODE"]) == "")
                            {
                                sErrorMsg += "품목";
                            }
                            if (Convert.ToString(drrow["POQTY"]) == "")
                            {
                                sErrorMsg += "발주 수량";
                            }
                            if (Convert.ToString(drrow["CUSTCODE"]) == "")
                            {
                                sErrorMsg += "거래처";
                            }
                            if (sErrorMsg != "")
                            {
                                this.ClosePrgForm();
                                ShowDialog(sErrorMsg + "을 입력하지 않았습니다",DC00_WinForm.DialogForm.DialogType.OK);
                                helper.Rollback();
                                return;
                            }

                            helper.ExecuteNoneQuery("06MM_StockMM_I1"
                                                    , CommandType.StoredProcedure
                                                    , helper.CreateParameter("PLANTCODE",  Convert.ToString(drrow["PLANTCODE"]),DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("ITEMCODE",   Convert.ToString(drrow["ITEMCODE"]), DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("POQTY",      Convert.ToString(drrow["POQTY"]),    DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("UNITCODE",   Convert.ToString(drrow["UNITCODE"]), DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("CUSTCODE",   Convert.ToString(drrow["CUSTCODE"]), DbType.String, ParameterDirection.Input)
                                                    , helper.CreateParameter("MAKER",      LoginInfo.UserID,                    DbType.String, ParameterDirection.Input)
                                                    );
                            break;
                        case DataRowState.Modified:
                            helper.ExecuteNoneQuery("06MM_StockMM_U1"
                        , CommandType.StoredProcedure
                        , helper.CreateParameter("PLANTCODE",  Convert.ToString(drrow["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                        , helper.CreateParameter("PONO",   Convert.ToString(drrow["PONO"]), DbType.String, ParameterDirection.Input)
                        , helper.CreateParameter("INQTY", Convert.ToString(drrow["INQTY"]), DbType.String, ParameterDirection.Input)
                        , helper.CreateParameter("EDITOR",     LoginInfo.UserID, DbType.String, ParameterDirection.Input));
                            break;
                    }
                }
                if (helper.RSCODE == "S")
                {
                    string s = helper.RSMSG;
                    helper.Commit();
                    this.ShowDialog("정상적으로 등록 되었습니다", DC00_WinForm.DialogForm.DialogType.OK);
                    DoInquire();
                }

            }
            catch (Exception ex)
            {
                helper.Rollback();
            }
            finally
            {
                helper.Close();
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            // 바코드 발행
            if (grid1.ActiveRow == null) return; // 선택된 행이 없을 경우 종료
            DataRow drRow = ((DataTable)this.grid1.DataSource).NewRow();
            // ROW의 이름은 GRID에 있는 이름이랑 같아야함
            drRow["ITEMCODE"] = Convert.ToString(this.grid1.ActiveRow.Cells["ITEMCODE"].Value);
            drRow["ITEMNAME"] = Convert.ToString(this.grid1.ActiveRow.Cells["ITEMNAME"].Value);
            drRow["CUSTNAME"] = Convert.ToString(this.grid1.ActiveRow.Cells["CUSTNAME"].Value);
            drRow["STOCKQTY"] = Convert.ToString(this.grid1.ActiveRow.Cells["STOCKQTY"].Value);
            drRow["MATLOTNO"] = Convert.ToString(this.grid1.ActiveRow.Cells["MATLOTNO"].Value);
            drRow["UNITCODE"] = Convert.ToString(this.grid1.ActiveRow.Cells["UNITCODE"].Value);

            // 바코드 디자인 선언
            Report_LotBacode repBarCode = new Report_LotBacode();
            // 레포트 북 선언
            Telerik.Reporting.ReportBook repBook = new Telerik.Reporting.ReportBook();
            // 바코드 디자이너에 데이터 등록
            repBarCode.DataSource = drRow;
            //레포트 북에 디자이너 등록
            repBook.Reports.Add(repBarCode);
            
            //미리보기 창 활성화
            ReportViewer repViewer = new ReportViewer(repBook, 1);
            repViewer.ShowDialog();
        }
    }
}
