var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var StuffCode = GetQueryString("code");
var toptoolbar = null;

$(function () {
    InitControl();
    Search();
});

function Search() {
    //导出数据成功后加载数据
    var Tid = $("#navtab1").ligerTab().getSelectedTabItemID(); //获取选择项ID

    var arr = new Array();
    arr[0] = StuffCode;
    arr[1] = Tid.replace("tabitem", "");
    arr[2] = PageIndex;
    arr[3] = PageNum;

    if (Tid == 'tabitem1') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "ValidData");
    }
    else if (Tid == 'tabitem2') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "AlreadyUser");
    }
    else if (Tid == 'tabitem3') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "UnsubscribeUser");
    }
    else if (Tid == 'tabitem4') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "AlreadyCallData");
    }
    else if (Tid == 'tabitem5') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "LoopData");
    }
    else if (Tid == 'tabitem6') {
        GridLoadIDAll("BLL_OriginalDataDts", "GetXFBBlackData", "GetXFBBlackDataCount", arr, "BlackUser");
    } 

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //点击选项加载数据
    $("#navtab1").ligerTab({
        onAfterSelectTabItem: function (tabid) {
            PageIndex = 1;
            var arr = new Array();
            arr[0] = StuffCode;
            arr[1] = tabid.replace("tabitem", "'");
            arr[2] = PageIndex;
            arr[3] = PageNum;
            if (tabid == 'tabitem1') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "ValidData");
            }
            else if (tabid == 'tabitem2') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "AlreadyUser");
            }
            else if (tabid == 'tabitem3') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "UnsubscribeUser");
            }
            else if (tabid == 'tabitem4') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "AlreadyCallData");
            }
            else if (tabid == 'tabitem5') {
                GridLoadIDAll("BLL_OriginalDataDts", "GetXFBViewData", "GetXFBViewDataCount", arr, "LoopData");
            }
            else if (tabid == 'tabitem6') {
                var arr = new Array();
                arr[0] = StuffCode;
                arr[1] = PageIndex;
                arr[2] = PageNum;
                GridLoadIDAll("BLL_OriginalDataDts", "GetXFBBlackData", "GetXFBBlackDataCount", arr, "BlackUser");
            } 
        }
    })
}

function InitControl() {
    //初始化工具条
    toptoolbar = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '开始筛选', icon: 'search2', click: GeneralDatas },
            { line: true },
            { text: '导出', icon: 'up', click: OutExcel }
        ]
    });

    //初始化选项
    var Tid = $("#navtab1").ligerTab().getSelectedTabItemID(); //获取选择项ID

    if (Tid == 'tabitem1') {
        gridManager = $("#ValidData").ligerGrid({ 
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
            ], pageSize: 20, width: '99%', height: '98%',
            pageSizeOptions: 20,
            enabledSort: false,
            usePager: true,
            onToFirst: onToFirst,
            onToPrev: onToPrev,
            onToNext: onToNext,
            onToLast: onToLast
        })
    }
    else if (Tid == 'tabitem2') {
        gridManager = $("#AlreadyUser").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
            ]
        })
    }
    else if (Tid == 'tabitem3') {
        gridManager = $("#UnsubscribeUser").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '联系电话', name: 'ODD_Phone', minWidth: 90 },
                { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
            ]
        })
    }
    else if (Tid == 'tabitem4') {
        gridManager = $("#AlreadyCallData").ligerGrid({ 
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
            ], pageSize: 20, width: '99%', height: '98%',
            pageSizeOptions: 20,
            enabledSort: false,
            usePager: true,
            onToFirst: onToFirst,
            onToPrev: onToPrev,
            onToNext: onToNext,
            onToLast: onToLast
        })
    }
    else if (Tid == 'tabitem5') {
        gridManager = $("#LoopData").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
            ]
        })
    }
    else if (Tid == 'tabitem6') {
        gridManager = $("#BlackUser").ligerGrid({
            columns: [
                { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                { display: '联系电话', name: 'ODD_Phone', minWidth: 90 },
                { display: '备注信息', name: 'RB_Provider', minWidth: 90 }
            ]
        })
    }
     
    GridInitIDControl("AlreadyUser");
    GridInitIDControl("UnsubscribeUser"); 
    GridInitIDControl("LoopData");
    GridInitIDControl("BlackUser");  

    ///////////////////////////////////////////////////////////////
    //初始化选项点击事件
    $("#navtab1").ligerTab({
        onAfterSelectTabItem: function (tabid) {
            if (tabid == 'tabitem1') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "block")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "block")
                gridManager = $("#ValidData").ligerGrid({ 
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
                    ], pageSize: 20, width: '99%', height: '98%',
                    pageSizeOptions: 20,
                    enabledSort: false,
                    usePager: true,
                    onToFirst: onToFirst,
                    onToPrev: onToPrev,
                    onToNext: onToNext,
                    onToLast: onToLast
                })
            }
            else if (tabid == 'tabitem2') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#AlreadyUser").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
                    ]
                })
            }
            else if (tabid == 'tabitem3') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#UnsubscribeUser").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '联系电话', name: 'ODD_Phone', minWidth: 90 },
                        { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
                    ]
                })
            }
            else if (tabid == 'tabitem4') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "block")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "block")
                gridManager = $("#AlreadyCallData").ligerGrid({ 
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
                    ], pageSize: 20, width: '99%', height: '98%',
                    pageSizeOptions: 20,
                    enabledSort: false,
                    usePager: true,
                    onToFirst: onToFirst,
                    onToPrev: onToPrev,
                    onToNext: onToNext,
                    onToLast: onToLast
                })
            }
            else if (tabid == 'tabitem5') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#LoopData").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '联系电话', name: 'ODD_Phone', minWidth: 90 }
                    ]
                })
            }
            else if (tabid == 'tabitem6') {
                $("#toptoolbar div[toolbarid='item-3']").css("display", "none")
                $("#toptoolbar div[toolbarid='item-3']").next().css("display", "none")
                gridManager = $("#BlackUser").ligerGrid({
                    columns: [
                        { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
                        { display: '联系电话', name: 'ODD_Phone', minWidth: 90 },
                        { display: '备注信息', name: 'RB_Provider', minWidth: 90 }
                    ]
                })
            } 

            GridInitIDControl("AlreadyUser");
            GridInitIDControl("UnsubscribeUser"); 
            GridInitIDControl("LoopData");
            GridInitIDControl("BlackUser"); 
        }
    })
}

//导出
function OutExcel() {
    var arr = new Array();

    var Tid = $("#navtab1").ligerTab().getSelectedTabItemID();
    arr[0] = StuffCode;
    arr[1] = "XFB_";
    if (Tid == 'tabitem1') {
        arr[1] = arr[1] + "ValidData";
    }
    else if (Tid == 'tabitem2') {
        arr[1] = arr[1] + "AlreadyUse";
    }
    else if (Tid == 'tabitem3') {
        arr[1] = arr[1] + "UnsubscribeUser";
    }
    else if (Tid == 'tabitem4') {
        arr[1] = arr[1] + "AlreadyCallData";
    }
    else if (Tid == 'tabitem5') {
        arr[1] = arr[1] + "LoopData";
    }
    else if (Tid == 'tabitem6') {
        arr[1] = arr[1] + "BlackUser";
    } 

    var data = ReturnValue("BLL_OrigDataImport", "DtToExcel", arr, Address);
    if (data == "true")
        $.ligerDialog.open({ url: "../../Aspx/ImportHelper.aspx" });
    else
        $.ligerDialog.error('当前内容为空,无法导出！');
}

//筛选
function GeneralDatas() {
    var arr = new Array();
    arr[0] = StuffCode;
    ReturnMethod("BLL_OriginalDataDts", "GeneralXFBDatas", arr, Address, "GeneralDataSuccess");
}

//提交
function GeneralVaildData() {
    var arr = new Array();
    arr[0] = StuffCode;
    ReturnMethod("BLL_OriginalDataDts", "SaveXFBGeneralDatas", arr, Address, "SaveDataSuccess");
}

//移动
function Move() {
    //初始化选项
    var Tid = $("#navtab1").ligerTab().getSelectedTabItemID(); //获取选择项ID
    var odCodes = checkedKeyWordsData.join('/');

    if (odCodes == "") {
        $.ligerDialog.error('请选择移动列！');
        return false;
    }
    var butn = '';
    if (Tid == "tabitem1") {
        butn = '<input name="type" type="radio" value="4" />已呼过&nbsp;&nbsp;<input name="type" type="radio" value="7" />关键字';
    } else if (Tid == "tabitem4") {
        butn = '<input name="type" type="radio" value="1" />有效数据&nbsp;&nbsp;<input name="type" type="radio" value="7" />关键字';
    } else if (Tid == "tabitem7") {
        butn = '<input name="type" type="radio" value="1" />有效数据&nbsp;&nbsp;<input name="type" type="radio" value="4" />已呼过';
    }

    layer.open({
        type: 1,
        title: "移动",
        skin: 'layui-layer-rim', //加上边框
        area: ['360px', '180px'], //宽高
        content: '<div style="margin-top:20px;padding:0 90px;">' + butn + '</div>'
        , btn: ['确定', '取消']
        , yes: function (index, layero) {
            var arr = new Array;
            arr[0] = odCodes;
            arr[1] = $("input[name='type']:checked").val();
            arr[2] = StuffCode;
            arr[3] = (Tid == "tabitem7" ? 1 : 0);//是否是关键字移动过来的
            var result = ReturnValue("BLL_OriginalDataDts", "MoveXFBScreenData", arr, Address);
            if (result == "true") {
                $.ligerDialog.success('移动成功！');
                checkedKeyWordsData = [];
                Search();
                layer.close(index);
            } else {
                $.ligerDialog.error('移动失败！');
            }
        }, btn2: function (index, layero) {
            layer.close(index);
        }
        , cancel: function () {

        }
    });
}

//提示方法
var CustomMethod = {
    SaveDataSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('提交成功！', '对话框', End)
        }
        else {
            $.ligerDialog.error('提交失败！');
        }
    },
    GeneralDataSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('筛选成功！');
        }
        else {
            $.ligerDialog.error('筛选失败！');
        }
        Search();
    },
    MoveSucess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('移动成功！');
            Search();
        }
        if (text == "false") {
            $.ligerDialog.error('移动失败！', '', function () { return false });
        }
    }
}

//关闭
function End() {
    parent.layer.closeAll('iframe');
}

function f_onCheckAllRow(checked) {
    for (var rowid in this.records) {
        if (checked)
            addCheckedCode(this.records[rowid]['ODD_Code']);
        else
            removeCheckedCode(this.records[rowid]['ODD_Code']);
    }
}
function findCheckedCode(Code) {
    for (var i = 0; i < checkedKeyWordsData.length; i++) {
        if (checkedKeyWordsData[i] == Code) return i;
    }
    return -1;
}

var checkedKeyWordsData = [];
function findCheckedCode(Code) {
    for (var i = 0; i < checkedKeyWordsData.length; i++) {
        if (checkedKeyWordsData[i] == Code) return i;
    }
    return -1;
}
function addCheckedCode(Code) {
    if (findCheckedCode(Code) == -1)
        checkedKeyWordsData.push(Code);
}
function removeCheckedCode(Code) {
    var i = findCheckedCode(Code);
    if (i == -1) return;
    checkedKeyWordsData.splice(i, 1);
}
function f_isChecked(rowdata) {
    if (findCheckedCode(rowdata.ODD_Code) == -1)
        return false;
    return true;
}
function f_onCheckRow(checked, data) {
    if (checked) addCheckedCode(data.ODD_Code);
    else removeCheckedCode(data.ODD_Code);
}