var Address = "../";
var gridManager = null;
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 20;
var MaxPage = 0;
var MaxCount = 0; 

$(function () {
    InitControl();
    InitData();
}); 

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '查询', id: "btnsearch", icon: 'search', click: SearchData },
            { line: true },
            { text: '审核', id: 'btnExam', icon: 'comment', click: Exam },
            //{ line: true },
            //{ text: '修改', id: 'btnModify', icon: 'modify', click: Modify },
            { line: true },
            { text: '查看', id: 'btnView', icon: 'view', click: View },
            { line: true }
        ]
    });
    GetMenuRight(toptoolbarManager, "2016112311928");
    $("#CustOpenDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 163 });
    $("#CustEndDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 163 });

    //初始化客户状态
    var StateData = eval("[{\"Cset_Name\":\"待审\",\"Cset_Code\":\"待审\"},{\"Cset_Name\":\"已审\",\"Cset_Code\":\"已审\"},{\"Cset_Name\":\"退订\",\"Cset_Code\":\"退订\"},{\"Cset_Name\":\"退费\",\"Cset_Code\":\"退费\"}]");
    //var StateData = eval(ReturnValue("BLL_CustomerService", "GetState", "", Address));
    ligerComboBoxs = $("#CustState").ligerComboBox({
        width: 162,
        selectBoxWidth: 162,
        selectBoxHeight: 120,
        //textField: 'CSet_Name', valueField: 'CSet_Code',
        textField: "Cset_Name",
        valueField: "Cset_Code",
        isMultiSelect: true,
        initValue: "待审", 
        data: StateData
    });

    gridManager = $("#grid").ligerGrid({
        columns: [
            {
                display: '', width: 30, render: function (rowdata, rowindex, value) {
                    var s = ""; 
                    if (rowdata.Cust_ReturnContent != "" && rowdata.Cust_ReturnContent != undefined) {
                        s = "<a href='#' title='" + rowdata.Cust_ReturnContent + "' id='tip' ><img height= '24'  width='24' style='cursor:pointer' src='../Images/jg.png'  /></a>";
                    }
                    return s;
                }
            },
            { display: '客户名称', name: 'Cust_Name', width: 120 },
            { display: '座机号码', name: 'Cust_Phone', minWidth: 90 },
            { display: '联系人', name: 'Cust_Linkman', minWidth: 90 },
            { display: '联系电话', name: 'Cust_LinkPhone', minWidth: 90 },
            { display: '计费号码', name: 'Cust_BillNumber', minWidth: 90 },
            { display: '是否计费', name: 'Cust_IsBill', minWidth: 70 },
            {
                display: '修改详情', name: 'XqCount', minWidth: 70
                , render: function (rowdata, rowindex, value) {
                    var h = "";
                    if (rowdata.XqCount == 0) {
                        h = "";

                    } else {
                        h = "<a href='javascript:ShowAtt(\"" + rowdata.Cust_Code + "\")'>详情</a>";
                    }
                    return h;
                }

            },
            { display: '客户状态', name: 'Cust_State', minWidth: 70 },
            { display: '开通时间', name: 'Cust_OpenDate', minWidth: 140 },
            { display: '录入时间', name: 'JoinDate', minWidth: 140 },
            { display: '录音编码', name: 'Cust_KFVoice', width: 160 }
        ],
        pageSize: PageNum, width: '98%', height: '98%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        usePager: true,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });
}

function InitData() {
    Search(); 
}

function SearchData() {
    onToFirst();
}

//查询
function Search() {
    var arr = new Array();
    arr[0] = $("#CustName").val();             //客户姓名
    arr[1] = $("#CustPhone").val();            //座机号码   
    arr[2] = $("#CustBillNumber").val();            //
    arr[3] = $("#CustLinkman").val();
    arr[4] = $("#CustLinkPhone").val();
    arr[5] = $("#CustState").val();
    arr[6] = $("#CustOpenDate").val();
    arr[7] = $("#CustEndDate").val();
    arr[8] = PageIndex;
    arr[9] = PageNum;

    MaxCount = ReturnValue("BLL_CustomerService", "GetCustomerCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetCustomer";
    var BLL = "BLL_CustomerService";
    gridManager = $("#grid").ligerGrid({
        url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });

}

// 审核
function Exam() {
    var row = gridManager.getSelected();
    if (!row) {
        $.ligerDialog.warn("请选择一条记录");
        return;
    }
    if (row.Cust_State != "待审") {
        $.ligerDialog.warn("非待审状态，无需审核！");
        return;
    }
    top.layer.open({
        type: 2,
        title: '审核',
        shadeClose: false,
        shade: 0.3,  
        area: ['85%', '75%'],
        content: '../Form/frm_CustomerServiceAudit.htm?Code=' + row.Cust_Code + '&State=Exam', //iframe的url
        end: function () {
            Search();
        }
    });
}

//修改
function Modify() {
    var row = gridManager.getSelected();
    if (!row) {
        $.ligerDialog.warn("请选择一条记录");
        return;
    }
    if (row.Cust_State != "已审") {
        $.ligerDialog.warn("非已审状态，不能修改！");
        return;
    }
    var str = '';
    if (row.Cust_IsBill == "是") {
        str = '<option value="0" selected="true">是</option><option value="1">否</option>';
    } else {
        str = '<option value="0" >是</option><option value="1" selected="true">否</option>';
    }
    layer.open({
        type: 1,
        title: "修改",
        skin: 'layui-layer-rim', //加上边框
        area: ['410px', '280px'], //宽高
        content: '<div style="margin-top:20px;padding:0 70px;"><label style="margin-right:13px;">是否计费</label><select id="selIsBill" style="width:200px;">' + str + '</select></div><div style="margin-top:20px;padding:0 70px;"><label style="margin-right:12px;vertical-align: top;">修改原因</label><textarea id="XGYY" style="width:200px;height:100px;" maxlength="100";></textarea></div>'
, btn: ['确定', '取消']
  , yes: function (index, layero) {
      var arr = new Array;
      arr[0] = row.Cust_Code;
      arr[1] = $("#selIsBill").val();
      arr[2] = $("#XGYY").val();
      var result = ReturnValue("BLL_CustomerService", "UpBillHistory", arr, Address);
      if (result == "true") {
          layer.msg('修改成功!', { icon: 1 });
          Search();
          layer.close(index); 
      } else {
          layer.msg('修改失败!', { icon: 1 });
      }
  }, btn2: function (index, layero) {
      layer.close(index); 
  }
  , cancel: function () {

  }
    });
}

//查看
function View() {
    var row = gridManager.getSelected();
    if (row) {
        var Code = row.Cust_UserCode;
        top.layer.open({
            type: 2,
            title: '查看',
            shadeClose: false,
            shade: 0.3,
            area: ['85%', '75%'],
            content: '../Form/frm_CustomerServiceAudit.htm?Code=' + row.Cust_Code + '&State=View', //iframe的url
            end: function () {
                Search();
            }
        });
        Search();
    }
    else {
        $.ligerDialog.warn("请选择一条记录");
        return;
    }
}

// 查看详情
function ShowAtt(code) {
    top.layer.open({
        type: 2,
        title: '修改详情',
        shadeClose: false,
        shade: 0.3,
        area: ['50%', '55%'],
        content: '../Form/frm_BillHistory.htm?Code=' + code, //iframe的url
        end: function () {
            Search();
        }
    });
}

function onToFirst() {
    PageIndex = 1;
    Search();
    return false;
}
//末页
function onToLast() {
    PageIndex = MaxPage;
    Search();
    return false;
}
//上一页
function onToPrev() {
    if (PageIndex - 1 > 0) {
        PageIndex = PageIndex - 1;
    } else {
        PageIndex = 1;
    }
    Search();
    return false;
}
//下一页
function onToNext() {
    if (PageIndex + 1 < MaxPage) {
        PageIndex = PageIndex + 1;
    } else {
        PageIndex = MaxPage;
    }
    Search();
    return false;
}





