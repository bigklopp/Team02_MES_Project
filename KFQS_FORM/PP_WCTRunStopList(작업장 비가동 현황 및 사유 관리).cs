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
    public partial class PP_WCTRunStopList : DC00_WinForm.BaseMDIChildForm
    {
        // 그리드를 세팅 할 수 있드록 도와주는 함수 클래스
        UltraGridUtil _GridUtil = new UltraGridUtil();
        // 공장 변수 입력
        private string sPlantCode = LoginInfo.UserID;

        public PP_WCTRunStopList()
        {
            InitializeComponent();
        }

        private void PP_WCTRunStopList_Load(object sender, EventArgs e)
        {
            // 그리드 세팅
            try
            {

                _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
                _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE"                , "공장"          , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "RSSEQ"                    , "RSSEQ"         , true, GridColDataType_emu.Integer, 130, 130, Infragistics.Win.HAlign.Left, false, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE"           , "작업장"        , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME"           , "작업장명"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ORDERNO"                  , "작업지시번호"  , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE"                 , "품목코드"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME"                 , "품명"          , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKER"                   , "작업자"        , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "WORKSTATUS"               , "가동/비가동"   , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Center, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "RSSTARTDATE"              , "시작일시"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "RSENDDATE"                , "종료일시"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "DURATION"                 , "소요시간(분)"  , true, GridColDataType_emu.Double, 130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "PRODQTY"                  , "양품 수량"     , true, GridColDataType_emu.Double, 130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "BADQTY"                   , "불량 수량"     , true, GridColDataType_emu.Double, 130, 130, Infragistics.Win.HAlign.Right, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "REMARK"                   , "사유"          , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, true);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE"                 , "등록일시"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "MAKER"                    , "공장"          , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "EDITDATE"                 , "수정일시"      , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
                _GridUtil.InitColumnUltraGrid(grid1, "EDITOR"                   , "수정자"        , true, GridColDataType_emu.VarChar, 130, 130, Infragistics.Win.HAlign.Left, true, false);
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


                dtTemp = _Common.GET_Workcenter_Code(); // 작업장

                //콤보박스 컨트롤에 가져온 데이터 등록
                Common.FillComboboxMaster(this.cboWorkCenter, dtTemp, dtTemp.Columns["CODE_ID"].ColumnName, dtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");


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
                string sWorkCenter = Convert.ToString(cboWorkCenter.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", dtpStart.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", dtpEnd.Value);

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("06PP_WCTRunStopList_S1", CommandType.StoredProcedure, helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input)
                                                                                               , helper.CreateParameter("WORKCENTERCODE", sWorkCenter, DbType.String, ParameterDirection.Input)
                                                                                               , helper.CreateParameter("STARTDATE", sStartDate, DbType.String, ParameterDirection.Input)
                                                                                               , helper.CreateParameter("ENDDATE", sEndDate, DbType.String, ParameterDirection.Input));

                this.ClosePrgForm();

                if (dtTemp.Rows.Count > 0)
                {
                    grid1.DataSource = dtTemp;
                    grid1.DataBinds(dtTemp);
                    this.grid1.DisplayLayout.Override.MergedCellContentArea = Infragistics.Win.UltraWinGrid.MergedCellContentArea.VirtualRect;
                    this.grid1.DisplayLayout.Bands[0].Columns["PLANTCODE"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                    this.grid1.DisplayLayout.Bands[0].Columns["WORKCENTERCODE"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                    this.grid1.DisplayLayout.Bands[0].Columns["WORKCENTERNAME"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                    this.grid1.DisplayLayout.Bands[0].Columns["ORDERNO"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                    this.grid1.DisplayLayout.Bands[0].Columns["ITEMCODE"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
                    this.grid1.DisplayLayout.Bands[0].Columns["ITEMNAME"].MergedCellStyle = Infragistics.Win.UltraWinGrid.MergedCellStyle.Always;
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
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {

                    helper.ExecuteNoneQuery("06PP_WCTRunStopList_U1", CommandType.StoredProcedure
                                                , helper.CreateParameter("PLANTCODE", Convert.ToString(dtTemp.Rows[i]["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("WORKCENTERCODE", Convert.ToString(dtTemp.Rows[i]["WORKCENTERCODE"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("ORDERNO", Convert.ToString(dtTemp.Rows[i]["ORDERNO"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("RSSEQ", dtTemp.Rows[i]["RSSEQ"], DbType.Int64, ParameterDirection.Input)
                                                , helper.CreateParameter("REMARK", Convert.ToString(dtTemp.Rows[i]["REMARK"]), DbType.String, ParameterDirection.Input)
                                                , helper.CreateParameter("EDITOR", LoginInfo.UserID, DbType.String, ParameterDirection.Input));

                    if (helper.RSCODE == "E")
                    {
                        this.ShowDialog(helper.RSMSG, DC00_WinForm.DialogForm.DialogType.OK);
                        helper.Rollback();
                        return;
                    }

                    helper.Commit();
                    this.ShowDialog(helper.RSMSG, DC00_WinForm.DialogForm.DialogType.OK);
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
                DoInquire();
                helper.Close();
            }
        }

        private void grid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            // 그리드 컬럼간 머지 합병 기능 적용
            CustomMergedCellEvalutor CM1 = new CustomMergedCellEvalutor("ORDERNO", "ITEMCODE");
            e.Layout.Bands[0].Columns["ITEMCODE"].MergedCellEvaluator = CM1;
            e.Layout.Bands[0].Columns["ITEMNAME"].MergedCellEvaluator = CM1;
        }
    }
}
