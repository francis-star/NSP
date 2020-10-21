var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var StuffCode = GetQueryString("code");
var toptoolbar = null;
var treeManager;

var columnDefault = [
    { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
    { display: '计费号码（预）', name: 'ODD_Phone', minWidth: 90 }
];
var columnOther = [
    { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
    { display: '计费号码（预）', name: 'ODD_Phone', minWidth: 90 },
    { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
];
var columnOtherTD = [
    { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
    { display: '计费号码（预）', name: 'ODD_Phone', minWidth: 90 },
    { display: '新增时间', name: 'AddDate', minWidth: 90 },
    { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
];
var columnOtherTD2 = [
    { display: '客户名称', name: 'ODD_Name', minWidth: 90 },
    { display: '计费号码（预）', name: 'ODD_Phone', minWidth: 90 },
    { display: '新增时间', name: 'AddDate', minWidth: 90 },
    { display: '退订时间', name: 'TDDate', minWidth: 90 },
    { display: '会员业务', name: 'ODD_Business', minWidth: 90 }
];

$(function () {
    InitControl();
    refreshTree();
    Search();
    $("#dvTypeTotal").html("有效数据：" + MaxCount);
});

function getData() {
    var arr = new Array();
    data = []
    arr[0] = StuffCode;
    var totalData = JSON.parse(ReturnValue("BLL_OriginalDataDts", "GetTotalCount", arr, Address));
    data.push({ id: 1, pid: 0, text: '有效数据' + (totalData.Rows[0].count > 0 ? '(' + totalData.Rows[0].count + ')' : "") });
    data.push({ id: 21, pid: 0, text: '可移动' + (totalData.Rows[12].count > 0 ? '(' + totalData.Rows[12].count + ')' : "") });
    data.push({ id: 22, pid: 0, text: '不可移动' + (totalData.Rows[13].count > 0 ? '(' + totalData.Rows[13].count + ')' : "") });

    data.push({ id: 4, pid: 21, text: '已呼过' + (totalData.Rows[3].count > 0 ? '(' + totalData.Rows[3].count + ')' : "") });
    data.push({ id: 10, pid: 21, text: '其他业务退订/退费' + (totalData.Rows[9].count > 0 ? '(' + totalData.Rows[9].count + ')' : "") });
    data.push({ id: 7, pid: 21, text: '关键字：低危' + (totalData.Rows[6].count > 0 ? '(' + totalData.Rows[6].count + ')' : "") });
    data.push({ id: 11, pid: 21, text: '无效' + (totalData.Rows[10].count > 0 ? '(' + totalData.Rows[10].count + ')' : "") });
    data.push({ id: 12, pid: 21, text: '名称相同退订/退费' + (totalData.Rows[11].count > 0 ? '(' + totalData.Rows[11].count + ')' : "") });

    data.push({ id: 5, pid: 22, text: '重复数据' + (totalData.Rows[4].count > 0 ? '(' + totalData.Rows[4].count + ')' : "") });
    data.push({ id: 6, pid: 22, text: '黑名单' + (totalData.Rows[5].count > 0 ? '(' + totalData.Rows[5].count + ')' : "") });
    data.push({ id: 9, pid: 22, text: '关键字：高危' + (totalData.Rows[8].count > 0 ? '(' + totalData.Rows[8].count + ')' : "") });
    data.push({ id: 2, pid: 22, text: '本业务会员' + (totalData.Rows[1].count > 0 ? '(' + totalData.Rows[1].count + ')' : "") });
    data.push({ id: 8, pid: 22, text: '其他业务会员' + (totalData.Rows[7].count > 0 ? '(' + totalData.Rows[7].count + ')' : "") });
    data.push({ id: 3, pid: 22, text: '本业务退订/退费' + (totalData.Rows[2].count > 0 ? '(' + totalData.Rows[2].count + ')' : "") });
    $("#dvTotal").html("结果分类：" + totalData.Rows[0].Total);
    return data;
}

function refreshTree() {
    tree = $("#tree1").ligerTree({
        data: getData(),
        idFieldName: 'id',
        slide: false,
        parentIDFieldName: 'pid',
        isExpand: true,
        checkbox: false,
        nodeWidth: 300,
        onSelect: onClick
    });
    treeManager = $("#tree1").ligerGetTreeManager();
    treeManager.expandAll();
    treeManager.selectNode($("#hdType").val());
}

function changeYear() {
    var sltValue = $("#txtYear").val() == "全部" ? "" : $("#txtYear").val();
    $("#hdYear").val(sltValue);
    Search();
}

function changeTDYear() {
    var sltValue = $("#txtTDYear").val() == "全部" ? "" : $("#txtTDYear").val();
    $("#hdTDYear").val(sltValue);
    Search();
}

function Search() {
    var hdValue = $("#hdType").val();
    var arr = new Array();
    arr[0] = StuffCode;
    arr[1] = hdValue;
    arr[2] = $("#hdYear").val();
    arr[3] = $("#hdTDYear").val();
    arr[4] = PageIndex;
    arr[5] = PageNum;

    var type = parseInt(hdValue);
    //可移动
    if (type == 1 || type == 4 || type == 7 || type == 10 || type == 11 || type == 12) {
        $('#ValidData').addClass('l-frozen')
        if (type < 8)//2列
        {
            gridManager = $("#ValidData").ligerGrid({
                checkbox: true,
                columns: columnDefault,
                pageSize: 20,
                width: '99%', height: '98%',
                pageSizeOptions: 20,
                enabledSort: false,
                usePager: true,
                onToFirst: onToFirst,
                onToPrev: onToPrev,
                onToNext: onToNext,
                onToLast: onToLast,
                isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
            })
        }
        else if (type == 12) {//3列 
            gridManager = $("#ValidData").ligerGrid({
                checkbox: true,
                columns: columnOther,
                pageSize: 20,
                width: '99%', height: '98%',
                pageSizeOptions: 20,
                enabledSort: false,
                usePager: true,
                onToFirst: onToFirst,
                onToPrev: onToPrev,
                onToNext: onToNext,
                onToLast: onToLast,
                isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
            })

        } else {//4列
            if (type == 10) {
                gridManager = $("#ValidData").ligerGrid({
                    checkbox: true,
                    columns: columnOtherTD2,
                    pageSize: 20,
                    width: '99%', height: '98%',
                    pageSizeOptions: 20,
                    enabledSort: false,
                    usePager: true,
                    onToFirst: onToFirst,
                    onToPrev: onToPrev,
                    onToNext: onToNext,
                    onToLast: onToLast,
                    isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
                })
            } else {
                gridManager = $("#ValidData").ligerGrid({
                    checkbox: true,
                    columns: columnOtherTD,
                    pageSize: 20,
                    width: '99%', height: '98%',
                    pageSizeOptions: 20,
                    enabledSort: false,
                    usePager: true,
                    onToFirst: onToFirst,
                    onToPrev: onToPrev,
                    onToNext: onToNext,
                    onToLast: onToLast,
                    isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
                })
            }
        }
    }
    else {//不可移动
        if (type == 8) {//3列
            gridManager = $("#ValidData").ligerGrid({
                checkbox: false,
                columns: columnOther,
                pageSize: 20,
                width: '99%', height: '98%',
                pageSizeOptions: 20,
                enabledSort: false,
                usePager: true,
                onToFirst: onToFirst,
                onToPrev: onToPrev,
                onToNext: onToNext,
                onToLast: onToLast
            })
        }
        else {//2列
            gridManager = $("#ValidData").ligerGrid({
                checkbox: false,
                columns: columnDefault,
                pageSize: 20,
                width: '99%', height: '98%',
                pageSizeOptions: 20,
                enabledSort: false,
                usePager: true,
                onToFirst: onToFirst,
                onToPrev: onToPrev,
                onToNext: onToNext,
                onToLast: onToLast
            })
        }
    }

    GridLoadIDAll("BLL_OriginalDataDts", "GetSSBViewData", "GetSSBViewDataCount", arr, "ValidData");
}

function InitControl() {
    //初始化工具条
    toptoolbar = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '开始筛选', icon: 'search2', click: GeneralDatas }
        ]
    });
    var manager = $("#btn2").ligerButton(
        {
            click: function () {
                OutExcel();
            }
        }
    );
    manager.setValue('导出');
    $("#lblSeleted").show();
}

function reSet() {
    $("#sltMove").empty();
    var opHtml = '<option value="0" selected="selected">请选择</option><option value="1">有效数据</option><option value="4">已呼过</option><option value="10">其他业务退订、退费</option><option value="7">关键字：低危</option><option value="11">无效</option><option value="12">名称相同退订/退费</option>';
    $("#sltMove").html(opHtml);
}

function moveData(data) {
    console.log(data)
    var type = $("#hdType").val();
    var moveType = $("#sltMove").val();
    if (parseInt(type) > 20 || $("#sltMove").val() == "0")
        return;
    if (checkedCustomer.length < 1) {
        $.ligerDialog.error('请选择移动行！');
        return false;
    }
    var arr = new Array;
    arr[0] = checkedCustomer.join('/');;
    arr[1] = moveType;
    arr[2] = StuffCode;
    arr[3] = moveType == "7" ? "1" : "";//是否是关键字移动过来的
    var result = ReturnValue("BLL_OriginalDataDts", "MoveSSBScreenData", arr, Address);
    if (result == "true") {
        $.ligerDialog.success('移动成功！');
        checkedCustomer = [];
        $("#spCount").html('');
        treeManager.setData(getData())
        Search();
        switchFun(parseInt(type));
        treeManager.selectNode(type);
    } else {
        $.ligerDialog.error('移动失败！');
    }
}

//导出
function OutExcel() {
    var arr = new Array();
    var treeNode = tree.getSelected();
    if (treeNode == null || tree.getSelected().data.id > 20) {
        $.ligerDialog.warn('请选择筛选树节点！');
        return;
    }
    else {
        var Tid = tree.getSelected().data.id;
        arr[0] = StuffCode;
        arr[1] = "SSB_";
        if (Tid == 1) {
            arr[1] = arr[1] + "ValidData";
        }
        else if (Tid == 2) {
            arr[1] = arr[1] + "AlreadyUse";
        }
        else if (Tid == 3) {
            arr[1] = arr[1] + "UnsubscribeUser";
        }
        else if (Tid == 4) {
            arr[1] = arr[1] + "AlreadyCallData";
        }
        else if (Tid == 5) {
            arr[1] = arr[1] + "LoopData";
        }
        else if (Tid == 6) {
            arr[1] = arr[1] + "BlackUser";
        }
        else if (Tid == 7) {
            arr[1] = arr[1] + "KeyWordData";
        }
        else if (Tid == 8) {
            arr[1] = arr[1] + "OtherData";
        }
        else if (Tid == 9) {
            arr[1] = arr[1] + "HighKeyWordData";
        }
        else if (Tid == 10) {
            arr[1] = arr[1] + "OtherDataTuiDing";
            arr[2] = $("#hdYear").val();
            arr[3] = $("#hdTDYear").val();
        }
        else if (Tid == 11) {
            arr[1] = arr[1] + "OtherNoValidData";
            arr[2] = $("#hdYear").val();
        }
        else
            arr[1] = arr[1] + "OtherNameCommonData";

        var data = ReturnValue("BLL_OrigDataImport", "DtToExcel", arr, Address);
        if (data == "true")
            $.ligerDialog.open({ url: "../../Aspx/ImportHelper.aspx" });
        else
            $.ligerDialog.error('当前内容为空,无法导出！');
    }
}

//筛选
function GeneralDatas() {
    layer.msg('加载中');
    var arr = new Array();
    arr[0] = StuffCode;
    ReturnMethod("BLL_OriginalDataDts", "GeneralSSBDatas", arr, Address, "GeneralDataSuccess");
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
            var result = ReturnValue("BLL_OriginalDataDts", "MoveSSBScreenData", arr, Address);
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
            location.reload()
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
            addCheckedCustomer(this.records[rowid]['ODD_Code']);
        else
            removeCheckedCustomer(this.records[rowid]['ODD_Code']);
    }
    $("#spCount").html(checkedCustomer.length);
}

var checkedCustomer = [];
function findCheckedCustomer(ODD_Code) {
    for (var i = 0; i < checkedCustomer.length; i++) {
        if (checkedCustomer[i] == ODD_Code) return i;
    }
    return -1;
}
function addCheckedCustomer(ODD_Code) {
    if (findCheckedCustomer(ODD_Code) == -1)
        checkedCustomer.push(ODD_Code);
}
function removeCheckedCustomer(ODD_Code) {
    var i = findCheckedCustomer(ODD_Code);
    if (i == -1) return;
    checkedCustomer.splice(i, 1);
}
function f_isChecked(rowdata) {
    if (findCheckedCustomer(rowdata.ODD_Code) == -1)
        return false;
    return true;
}
function f_onCheckRow(checked, data) {
    if (checked) addCheckedCustomer(data.ODD_Code);
    else removeCheckedCustomer(data.ODD_Code);

    $("#spCount").html(checkedCustomer.length);
} 