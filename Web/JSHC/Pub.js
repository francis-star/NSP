//模块按钮权限
function GetMenuRight(e, SMCode) {
    var arr = new Array();
    arr[0] = SMCode;

    var data = ReturnValue("BLL_Pub", "GetMenuRight", arr, Address);

    if (data != "") {
        var strMenu = data.split(',');
        //割线计数
        var IsLine = 0;
        for (var i = 0; i < e.options.items.length; i++) {
            for (var j = 0; j < strMenu.length; j++) {
                if (e.options.items[i].id == strMenu[j]) {
                    e.removeItem(strMenu[j]);
                    if (e.options.items[i + 1] != undefined) {
                        if (e.options.items[i + 1].line == true) {
                            $($("[class=l-bar-separator]")[IsLine--]).remove();
                        }
                    }
                    break;
                }
            }
            if (e.options.items[i].line == true) {
                IsLine++;
            }
        }
    }
}

//日期格式化
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(), //day 
        "h+": this.getHours(), //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
        "S": this.getMilliseconds() //millisecond 
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

//错误返回值
function ReturnErrorUrl() {
    if (top != self)
        top.location.href = Address + "login.htm";
    else
        location.href = Address + "login.htm";
}

//截取字符串 包含中文处理  
//(串,长度,增加...)  
function subString(str, len, hasDot) {
    var newLength = 0;
    var newStr = "";
    var chineseRegex = /[^\x00-\xff]/g;
    var singleChar = "";
    var strLength = str.replace(chineseRegex, "**").length;
    for (var i = 0; i < strLength; i++) {
        singleChar = str.charAt(i).toString();
        if (singleChar.match(chineseRegex) != null) {
            newLength += 2;
        }
        else {
            newLength++;
        }
        if (newLength > len) {
            break;
        }
        newStr += singleChar;
    }

    if (hasDot && strLength > len) {
        newStr += "...";
    }
    return newStr;
}

//开启模式窗口
function showMyModal() {
    var url = "${applicationScope.rootpath}html/functionofstore/locationfloat2.jsp" + "?r=" + Math.random();
    //传入参数示例

    var modalReturnValue = myShowModalDialog(url, window, 1030, 1945);
    //alert(modalReturnValue.name);
    //窗口关闭后执行某些方法
    //TODO　sth
}
//弹出框google Chrome执行的是open
function myShowModalDialog(url, args, width, height) {
    var tempReturnValue;
    if (navigator.userAgent.indexOf("Chrome") > 0) {
        var paramsChrome = 'height=' + height + ', width=' + width + ', top=' + (((window.screen.height - height) / 2) - 50) +
            ',left=' + ((window.screen.width - width) / 2) + ',toolbar=no, menubar=no, scrollbars=no, resizable=no, location=no, status=no';
        window.open(url, "newwindow", paramsChrome);
    }
    else {
        var params = 'dialogWidth:' + width + 'px;dialogHeight:' + height + 'px;status:no;dialogLeft:'
                    + ((window.screen.width - width) / 2) + 'px;dialogTop:' + (((window.screen.height - height) / 2) - 50) + 'px;';
        tempReturnValue = window.showModalDialog(url, args, params);
    }
    return tempReturnValue;
}