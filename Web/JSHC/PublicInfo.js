var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../"; //当前的html与Aspx中PubForm的文件深度
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;

$(function () {

    InitControl();
    
    InitData();

});

function InitControl() {

    $("#Inp_BeginTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 135 });

    $("#Inp_EndTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 135 });

    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                    { text: '查询', id:'btnsearch',icon: 'search', click: SearchData },
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
            { display: '标题', name: 'Pub_Title', minWidth: 30 },
            { display: '信息类别', name: 'LS_Name1', minWidth: 30 },
            { display: '信息小类', name: 'LS_Name2', minWidth: 40 },
            { display: '关键字', name: 'Pub_KeyWords', minWidth: 40 },
            { display: '创建时间', name: 'JoinDate', minWidth: 40 },
            { display: '编辑人', name: 'JoinMan', minWidth: 40 }
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

    $("#MobileTerminal").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value1",
        width: 135
    });

    $("#PlatformName").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value2",
        width: 135
    });

    $("#InformationCategory").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value3",
        width: 135,
        isMultiSelect: false
    });

    $("#InformationType").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value4",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

    $("#Inp_Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

    $("#Inp_City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

    $("#Inp_District").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value7",
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });

    GetMenuRight(toptoolbarManager, "2016112311935");
    
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


//获取移动端
function GetSearchStrData1() {
    var arr = new Array();
    arr[0] = "";
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    $("#MobileTerminal").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value1",
        value: Data[1].LS_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            PageIndex = 1;
            if (newvalue != null && newvalue != "") {

                $("#PlatformName").val("");
                $("#value2").val("");

                GetSearchStrData2(newvalue);
            }
        }
    });
}

//获取平台名称
function GetSearchStrData2(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    $("#PlatformName").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value2",
        value: Data[0].LS_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            $("#InformationCategory").val("");
            $("#value3").val("");
            if (newvalue != null && newvalue != "") {
                GetSearchStrData3(newvalue);
            } else {
                $("#InformationCategory").ligerComboBox({
                    data: "",
                    textField: "LS_Name",
                    valueField: "LS_Code",
                    valueFieldID: "value3",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

//获取类别
function GetSearchStrData3(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"LS_Code\": \"\", \"LS_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#InformationCategory").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value3",
        value: Data[0].LS_Code,
        width: 135,
        cancelable: false,
        isMultiSelect: false,
        onSelected: function (newvalue) {
            $("#InformationType").val("");
            $("#value4").val("");
            if (newvalue != null && newvalue != "") {
                GetSearchStrData4(newvalue);
            } else {
                $("#InformationType").ligerComboBox({
                    data: "",
                    textField: "LS_Name",
                    valueField: "LS_Code",
                    valueFieldID: "value4",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

//获取类别
function GetSearchStrData4(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "";
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetSearchStrData", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"LS_Code\": \"\", \"LS_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#InformationType").ligerComboBox({
        data: Data,
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value4",
        value: Data[0].LS_Code,
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });
}

function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择省份\"}]")[0]);//追加全国选项
    }
    $("#Inp_Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        value: Data[0].SA_Code,
        width: 135,
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
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
                $("#Inp_District").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value7",
                    width: 135,
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
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择城市\"}]")[0]);//追加全部选项
    }
    $("#Inp_City").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data[0].SA_Code,
        width: 135,
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
                    width: 135,
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
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择区县\"}]")[0]);//追加全部选项
    }
    $("#Inp_District").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value7",
        value: Data[0].SA_Code,
        width: 135,
        cancelable: false,
        isMultiSelect: false
    });
}

function InitData() {
    GetSearchStrData1();
    GetArea1();
    SearchData();
}

function Search() {
    var arr = new Array();
    if ($("#MobileTerminal").val() == "") {
        arr[0] = "";
    } else {
        arr[0] = $("#value1").val();
    }
    if ($("#PlatformName").val() == "") {
        arr[1] = "";
    } else {
        arr[1] = $("#value2").val();
    }
    if ($("#InformationCategory").val() == "") {
        arr[2] = "";
    } else {
        arr[2] = $("#value3").val();
    }
    arr[3] = $("#Inp_Title").val();
    arr[4] = "";
    if ($("#InformationType").val() == "") {
        arr[5] = "";
    } else {
        arr[5] = $("#value4").val();
    }
    if ($("#Inp_Province").val() == "") {
        arr[6] = "";
    } else {
        arr[6] = $("#value5").val();
    }
    if ($("#Inp_City").val() == "") {
        arr[7] = "";
    } else {
        arr[7] = $("#value6").val();
    }
    if ($("#Inp_District").val() == "") {
        arr[8] = "";
    } else {
        arr[8] = $("#value7").val();
    }
    arr[9] = ($("#Inp_BeginTime").val() == "" ? "" : $("#Inp_BeginTime").val() + " 00:00:00");
    arr[10] = ($("#Inp_EndTime").val() == "" ? "" : $("#Inp_EndTime").val() + " 23:59:59");
    arr[11] = PageIndex;
    arr[12] = PageNum;
    arr[13] = "0";
    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetPublicInfo";
    var BLL = "BLL_PublicInfo";
    gridManager = $("#grid").ligerGrid({
        url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
    });
    return false;
}

function SearchData() {
    PageIndex = 1;
    var arr = new Array();
    if ($("#MobileTerminal").val() == "") {
        arr[0] = "";
    } else {
        arr[0] = $("#value1").val();
    }
    if ($("#PlatformName").val() == "") {
        arr[1] = "";
    } else {
        arr[1] = $("#value2").val();
    }
    if ($("#InformationCategory").val() == "") {
        arr[2] = "";
    } else {
        arr[2] = $("#value3").val();
    }
    arr[3] = $("#Inp_Title").val();
    arr[4] = "";
    if ($("#InformationType").val() == "") {
        arr[5] = "";
    } else {
        arr[5] = $("#value4").val();
    }
    if ($("#Inp_Province").val() == "") {
        arr[6] = "";
    } else {
        arr[6] = $("#value5").val();
    }
    if ($("#Inp_City").val() == "") {
        arr[7] = "";
    } else {
        arr[7] = $("#value6").val();
    }
    if ($("#Inp_District").val() == "") {
        arr[8] = "";
    } else {
        arr[8] = $("#value7").val();
    }
    arr[9] = ($("#Inp_BeginTime").val() == "" ? "" : $("#Inp_BeginTime").val() + " 00:00:00");
    arr[10] = ($("#Inp_EndTime").val() == "" ? "" : $("#Inp_EndTime").val() + " 23:59:59");
    arr[11] = PageIndex;
    arr[12] = PageNum;
    arr[13] = "0";
    MaxCount = ReturnValue("BLL_PublicInfo", "GetPublicInfoCount", arr, Address);
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
    Search();
}

function AddData() {
    top.layer.open({
        type: 2,
        title: '添加资讯信息',
        shadeClose: false,
        shade: 0.3,
        area: ['90%', '90%'],
        content: '../Form/frm_PublicInfoDts.aspx?Code=',//iframe的url
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
            title: '修改资讯信息',
            shadeClose: false,
            shade: 0.3,
            area: ['90%', '90%'],
            content: '../Form/frm_PublicInfoDts.aspx?Code=' + row.Pub_Code, //iframe的url
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
                arr[0] = row.Pub_Code;
                var Data = ReturnValue("BLL_PublicInfo", "DeletePublicInfo", arr, Address);
                if (Data = "true") {
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