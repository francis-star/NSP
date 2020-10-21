var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;

$(function () {

    InitControl();
    InitData();

});

function InitControl() {

    $("#Inp_BeginTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 150 });

    $("#Inp_EndTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 150 });

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                    { text: '查询', id: 'btnsearch', icon: 'search', click: SearchData },
                    { line: true },
                    { text: '新增', id: 'btnImport', icon: 'add', click: AddData },
                    { line: true },
                    { text: '修改', id: 'btnUpdate', icon: 'pager', click: UpdateData },
                    { line: true },
                    { text: '删除', id: 'btnDelete', icon: 'edit', click: DeleteDatas },
                    { line: true },
                    {
                        text: '退出', icon: 'logout', click: function (item) {
                            top.navtab.removeTabItem('1018');
                        }
                    }
        ]
    });

    gridManager = $("#grid").ligerGrid({
        columns: [
            {
                display: '置顶', name: 'MD_TopDate', minWidth: 30, render: function (record, rowindex, value, column) {
                    var IsTop = 1;
                    var imgsrc = 'top1';
                    if (record.MD_TopDate != null && record.MD_TopDate != '') {
                        imgsrc = 'top';
                        IsTop = 0;
                    }
                    return "<img src='../../images/" + imgsrc + ".png' style='width:25px; cursor:pointer' onclick='topmd(\"" + record.MD_ID + "\"," + IsTop + ")'/>";
                }
            },
            { display: '投诉时间', name: 'MD_ComplaintTime', minWidth: 30 },
            { display: '标题', name: 'MD_Title', minWidth: 30 },
            { display: '投诉类型', name: 'MD_ComplaintType', minWidth: 40 },
            { display: '行业类型', name: 'MD_IndustryType', minWidth: 40 },
            { display: '处理单位', name: 'MD_ProcessingUnit', minWidth: 40 }
        ],
        
        pageSize: PageNum, width: '99%', height: '98%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        usePager: true,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast
    });

    $("#Inp_Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 150,
        cancelable: false,
        isMultiSelect: false
    });

    $("#Inp_City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 150,
        cancelable: false,
        isMultiSelect: false
    });

    $("#Inp_District").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value7",
        width: 150,
        cancelable: false,
        isMultiSelect: false
    });

    GetMenuRight(toptoolbarManager, "2016112311950");
}
function topmd(id, type) {
    var arr = new Array();
    arr[0] = id;
    arr[1] = type;
    ReturnValue("BLL_Mediation", "DoTopMediation", arr, Address);
    InitData();
}
//首页
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

//获取购买渠道
function GetComplaintchannel() {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = "Complaintchannel";
    arr[1] = "0";
    arr[2] = "0";
    var Data = eval(ReturnValue("BLL_Mediation", "GetParaType", arr, Address));
    
    $("#Inp_Complaintchannel").ligerComboBox({
        data: Data,
        textField: "CSet_Name",
        valueField: "CSet_Code",
        valueFieldID: "value_Complaintchannel",
        value: '',
        width: 150,
        isMultiSelect: true,
        cancelable: true,
    });
}

//获取投诉类型
function GetComplaintType() {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = "ComplaintType";
    arr[1] = "0";
    arr[2] = "0";
    var Data = eval(ReturnValue("BLL_Mediation", "GetParaType", arr, Address));
    
    $("#Inp_ComplaintType").ligerComboBox({
        data: Data,
        textField: "CSet_Name",
        valueField: "CSet_Code",
        valueFieldID: "value_ComplaintType",
        value: '',
        width: 150,
        cancelable: true,
        isMultiSelect: false,
        onSelected: function (newvalue) {
            $("#Inp_IndustryType").val("");
            $("#value_IndustryType").val("");
            if (newvalue != null && newvalue != "") {
                GetIndustryType(newvalue);
            } else {
                $("#Inp_IndustryType").ligerComboBox({
                    data: "",
                    textField: "CSet_Name",
                    valueField: "CSet_Code",
                    valueFieldID: "value_IndustryType",
                    width: 150,
                    cancelable: true,
                    isMultiSelect: true
                });
            }
        }
    });
}

//获取类别
function GetIndustryType(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "1";
    arr[2] = "0";
    var Data = eval(ReturnValue("BLL_Mediation", "GetParaType", arr, Address));

    $("#Inp_IndustryType").ligerComboBox({
        data: Data,
        textField: "CSet_Name",
        valueField: "CSet_Code",
        valueFieldID: "value_IndustryType",
        value: '',
        width: 150,
        cancelable: true,
        isMultiSelect: true
    });
}

function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_Mediation", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择省份\"}]")[0]);//追加全国选项
    }
    $("#Inp_Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        value: Data[0].SA_Code,
        width: 150,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            PageIndex = 1;
            if (newvalue != null && newvalue != "") {

                $("#Inp_City").val("");
                $("#value6").val("");

                GetArea2(newvalue);
            } else {
                $("#Inp_City").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value6",
                    width: 150,
                    cancelable: false,
                    isMultiSelect: false
                });
                $("#Inp_District").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value7",
                    width: 150,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

function GetArea2(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = '1';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_Mediation", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择城市\"}]")[0]);//追加全部选项
    }
    $("#Inp_City").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data[0].SA_Code,
        width: 150,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            $("#Inp_District").val("");
            $("#value7").val("");
            if (newvalue != null && newvalue != "") {
                GetArea3(newvalue);
            } else {
                $("#Inp_District").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value7",
                    width: 150,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

function GetArea3(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = '2';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_Mediation", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择区县\"}]")[0]);//追加全部选项
    }
    $("#Inp_District").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value7",
        value: Data[0].SA_Code,
        width: 150,
        cancelable: false,
        isMultiSelect: false
    });
}

function InitData() {
    GetComplaintchannel();
    GetComplaintType();
    GetArea1();
    SearchData();
}

function Search() {
    var arr = new Array();
    arr[0] = GetSearchFitter();
    arr[1] = 'MD_TopDate desc,JoinDate desc';
    arr[2] = PageIndex;
    arr[3] = PageNum;
    arr[4] = "0";
    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetMediation";
    var BLL = "BLL_Mediation";
    gridManager = $("#grid").ligerGrid({
        url: "../../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });
    return false;
}

function SearchData() {
    PageIndex = 1;
    var arr = new Array();
    var arr = new Array();
    arr[0] = GetSearchFitter();
    arr[1] = '';
    arr[2] = PageIndex;
    arr[3] = PageNum;
    arr[4] = "1";
    MaxCount = ReturnValue("BLL_Mediation", "GetMediationDataCount", arr, Address);
    //MaxCount = JSON.parse(MaxCount).total;
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
    Search();
}
function GetSearchFitter()
{
    var strSelect = ' 1=1 ';
    if ($("#value_Complaintchannel").val() != '' || $("#Inp_Complaintchannel").val() != '') {
        strSelect = strSelect + ' and MD_Complaintchannel_Value in (' + $("#value_Complaintchannel").val() + ')';
    }
    if ($("#value_ComplaintType").val() != '' || $("#Inp_ComplaintType").val() != '') {
        strSelect = strSelect + ' and MD_ComplaintType_Value=' + $("#value_ComplaintType").val();
    }
    if ($("#Inp_BeginTime").val() != '') {
        strSelect = strSelect + " and MD_ComplaintTime>='" + $("#Inp_BeginTime").val() + "'";
    }
    if ($("#Inp_EndTime").val() != '') {
        strSelect = strSelect + " and MD_ComplaintTime<='" + $("#Inp_EndTime").val() + "'";
    }
    if ($("#value_IndustryType").val() != '' || $("#Inp_IndustryType").val() != '') {
        strSelect = strSelect + ' and MD_IndustryType_Value in (' + $("#value_IndustryType").val() + ')';
    }
    if ($("#Inp_ProcessingUnit").val() != '') {
        strSelect = strSelect + " and MD_ProcessingUnit like '%" + $("#Inp_ProcessingUnit").val() + "%'";
    }
    if ($("#Inp_Province").val() != '请选择省份') {
        strSelect = strSelect + " and MD_Province='" + $("#Inp_Province").val() + "'";
    }
    if ($("#Inp_City").val() != '请选择城市' && $("#Inp_City").val() != '') {
        strSelect = strSelect + " and MD_City='" + $("#Inp_City").val() + "'";
    }
    if ($("#Inp_District").val() != '请选择区县'&&$("#Inp_District").val() != '') {
        strSelect = strSelect + " and MD_District='" + $("#Inp_District").val() + "'";
    }
    return encodeURI(strSelect);
}
function AddData() {
    top.layer.open({
        type: 2,
        title: '添加易调解',
        shadeClose: false,
        shade: 0.3,
        area: ['90%', '90%'],
        content: '../Form/315/frm_MediationMaintain.htm?Code=',//iframe的url
        end: function () {
            Search();
        }

    });
    //window.showModalDialog('frm_PublicInfoDts.aspx?Code=', null, "dialogWidth=900px;dialogHeight=500px");
}

function UpdateData() {
    var row = gridManager.getSelected();
    if (row) {
        top.layer.open({
            type: 2,
            title: '修改易调解',
            shadeClose: false,
            shade: 0.3,
            area: ['90%', '90%'],
            content: '../Form/315/frm_MediationMaintain.htm?Code=' + row.MD_Code, //iframe的url
            end: function () {
                Search();
            }
        });
        //window.showModalDialog('frm_PublicInfoDts.aspx?Code=' + row.Pub_Code, null, "dialogWidth=900px;dialogHeight=500px");
    }
    else {
        $.ligerDialog.warn("请选择一条记录!");
        return;
    }
}

function DeleteDatas() {
    var row = gridManager.getSelected();
    if (row) {
        $.ligerDialog.confirm('确定删除？', function (yes) {
            if (yes) {
                var arr = new Array();
                arr[0] = row.MD_Code;
                arr[1] = '';
                arr[2] = '';
                arr[3] = '';
                arr[4] = '';
                arr[5] = '';
                arr[6] = '';
                arr[7] = '';
                arr[8] = '';
                arr[9] = '';
                arr[10] = '';
                arr[11] = '';
                arr[12] = 3;
                var Data = ReturnValue("BLL_Mediation", "OperateMediation", arr, Address);
                if (Data = "1") {
                    $.ligerDialog.success("删除成功!");
                    SearchData();
                } else {
                    $.ligerDialog.error("删除失败!");
                }
            }
        });
    }
    else {
        $.ligerDialog.warn("请选择一条记录!");
        return;
    }
}

var CustomMethod = {
    GetPublicInfoSuccess: function (text) {
        var Data = eval("(" + text + ")");
        gridManager.loadData(Data);
        gridManager.changePage('first');

    }
}

//针对一个传参时，如果出现参数为空时，自动补充一个空参数，且中文逗号取代英文逗号
function ReplaceStr(para) {
    //针对一个传参时，如果出现参数为空时，自动补充一个空参数
    if (para.length == 1 && para[0] == "")
        para[1] = "";
    if (para.length > 0) {
        for (var i = 0; i < para.length; i++) {
            if (para[i] != undefined && isNaN(para[i])) {
                para[i] = para[i].replace(/,/g, '，');
            }
        }
    }
}