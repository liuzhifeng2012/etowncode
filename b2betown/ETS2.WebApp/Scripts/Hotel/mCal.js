(function () {
    //日历
    ChineseCalendar = function (dateObj) {
        this.dateObj = (dateObj != undefined) ? dateObj : new Date();
        this.SY = this.dateObj.getFullYear();
        this.SM = this.dateObj.getMonth();
        this.SD = this.dateObj.getDate();
        this.lunarInfo = [0x04bd8, 0x04ae0, 0x0a570, 0x054d5, 0x0d260, 0x0d950, 0x16554, 0x056a0, 0x09ad0, 0x055d2,
			0x04ae0, 0x0a5b6, 0x0a4d0, 0x0d250, 0x1d255, 0x0b540, 0x0d6a0, 0x0ada2, 0x095b0, 0x14977,
			0x04970, 0x0a4b0, 0x0b4b5, 0x06a50, 0x06d40, 0x1ab54, 0x02b60, 0x09570, 0x052f2, 0x04970,
			0x06566, 0x0d4a0, 0x0ea50, 0x06e95, 0x05ad0, 0x02b60, 0x186e3, 0x092e0, 0x1c8d7, 0x0c950,
			0x0d4a0, 0x1d8a6, 0x0b550, 0x056a0, 0x1a5b4, 0x025d0, 0x092d0, 0x0d2b2, 0x0a950, 0x0b557,
			0x06ca0, 0x0b550, 0x15355, 0x04da0, 0x0a5b0, 0x14573, 0x052b0, 0x0a9a8, 0x0e950, 0x06aa0,
			0x0aea6, 0x0ab50, 0x04b60, 0x0aae4, 0x0a570, 0x05260, 0x0f263, 0x0d950, 0x05b57, 0x056a0,
			0x096d0, 0x04dd5, 0x04ad0, 0x0a4d0, 0x0d4d4, 0x0d250, 0x0d558, 0x0b540, 0x0b6a0, 0x195a6,
			0x095b0, 0x049b0, 0x0a974, 0x0a4b0, 0x0b27a, 0x06a50, 0x06d40, 0x0af46, 0x0ab60, 0x09570,
			0x04af5, 0x04970, 0x064b0, 0x074a3, 0x0ea50, 0x06b58, 0x055c0, 0x0ab60, 0x096d5, 0x092e0,
			0x0c960, 0x0d954, 0x0d4a0, 0x0da50, 0x07552, 0x056a0, 0x0abb7, 0x025d0, 0x092d0, 0x0cab5,
			0x0a950, 0x0b4a0, 0x0baa4, 0x0ad50, 0x055d9, 0x04ba0, 0x0a5b0, 0x15176, 0x052b0, 0x0a930,
			0x07954, 0x06aa0, 0x0ad50, 0x05b52, 0x04b60, 0x0a6e6, 0x0a4e0, 0x0d260, 0x0ea65, 0x0d530,
			0x05aa0, 0x076a3, 0x096d0, 0x04bd7, 0x04ad0, 0x0a4d0, 0x1d0b6, 0x0d250, 0x0d520, 0x0dd45,
			0x0b5a0, 0x056d0, 0x055b2, 0x049b0, 0x0a577, 0x0a4b0, 0x0aa50, 0x1b255, 0x06d20, 0x0ada0,
			0x14b63];

        //传回农历 y年闰哪个月 1-12 , 没闰传回 0
        this.leapMonth = function (y) {
            return this.lunarInfo[y - 1900] & 0xf;
        };
        //传回农历 y年m月的总天数
        this.monthDays = function (y, m) {
            return (this.lunarInfo[y - 1900] & (0x10000 >> m)) ? 30 : 29;
        };
        //传回农历 y年闰月的天数
        this.leapDays = function (y) {
            if (this.leapMonth(y)) {
                return (this.lunarInfo[y - 1900] & 0x10000) ? 30 : 29;
            }
            else {
                return 0;
            }
        };
        //传回农历 y年的总天数
        this.lYearDays = function (y) {
            var i, sum = 348;
            for (i = 0x8000; i > 0x8; i >>= 1) {
                sum += (this.lunarInfo[y - 1900] & i) ? 1 : 0;
            }
            return sum + this.leapDays(y);
        };
        //算出农历, 传入日期对象, 传回农历日期对象
        //该对象属性有 .year .month .day .isLeap .yearCyl .dayCyl .monCyl
        this.Lunar = function (dateObj) {
            var i, leap = 0, temp = 0, lunarObj = {};
            var baseDate = new Date(1900, 0, 31);
            var offset = (dateObj - baseDate) / 86400000;
            lunarObj.dayCyl = offset + 40;
            lunarObj.monCyl = 14;
            for (i = 1900; i < 2050 && offset > 0; i++) {
                temp = this.lYearDays(i);
                offset -= temp;
                lunarObj.monCyl += 12;
            }
            if (offset < 0) {
                offset += temp;
                i--;
                lunarObj.monCyl -= 12;
            }

            lunarObj.year = i;
            lunarObj.yearCyl = i - 1864;
            leap = this.leapMonth(i);
            lunarObj.isLeap = false;
            for (i = 1; i < 13 && offset > 0; i++) {
                if (leap > 0 && i == (leap + 1) && lunarObj.isLeap == false) {
                    --i;
                    lunarObj.isLeap = true;
                    temp = this.leapDays(lunarObj.year);
                }
                else {
                    temp = this.monthDays(lunarObj.year, i)
                }
                if (lunarObj.isLeap == true && i == (leap + 1)) {
                    lunarObj.isLeap = false;
                }
                offset -= temp;
                if (lunarObj.isLeap == false) {
                    lunarObj.monCyl++;
                }
            }

            if (offset == 0 && leap > 0 && i == leap + 1) {
                if (lunarObj.isLeap) {
                    lunarObj.isLeap = false;
                }
                else {
                    lunarObj.isLeap = true;
                    --i;
                    --lunarObj.monCyl;
                }
            }

            if (offset < 0) {
                offset += temp;
                --i;
                --lunarObj.monCyl
            }
            lunarObj.month = i;
            lunarObj.day = offset + 1;
            return lunarObj;
        };
        //中文日期
        this.cDay = function (m, d) {
            var nStr1 = ['日', '一', '二', '三', '四', '五', '六', '七', '八', '九', '十'];
            var nStr2 = ['初', '十', '廿', '卅', '　'];
            var s;
            if (m > 10) {
                s = '十' + nStr1[m - 10];
            }
            else {
                s = nStr1[m];
            }
            s += '月';
            switch (d) {
                case 10:
                    s += '初十';
                    break;
                case 20:
                    s += '二十';
                    break;
                case 30:
                    s += '三十';
                    break;
                default:
                    s += nStr2[Math.floor(d / 10)];
                    s += nStr1[d % 10];
            }
            return s;
        };
        this.solarDay2 = function () {
            var sDObj = new Date(this.SY, this.SM, this.SD);
            var lDObj = this.Lunar(sDObj);
            var tt = (lDObj.month >= 10 ? lDObj.month : '0' + lDObj.month) + "" + (lDObj.day >= 10 ? lDObj.day : '0' + lDObj.day);
            lDObj = null;
            return tt;
        };
        this.weekday = function () {
            var day = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
            return day[this.dateObj.getDay()];
        };
        this.YYMMDD = function () {
            var dateArr = [this.SY, '年', this.SM + 1, '月', this.SD, '日'];
            return dateArr.join('');
        }
    };
    DisplayCalendar = function (dWrap, dObj, dStartInput, dBackInput, CheckInDate, CheckOutDate) {
        this.chineseHoliday = {
            '1230': '除夕',
            '0101': '春节',
            '0115': '元宵节',
            '0505': '端午节',
            '0707': '七夕',
            '0815': '中秋节',
            '0909': '重阳节'
        },
	this.holiday = {
	    '0101': '元旦',
	    '0214': '情人节',
	    '0404': '清明节',
	    '0501': '五一',
	    '1001': '国庆节',
	    '1225': '圣诞节'
	};
        this.aWeek = ['周日', '周一', '周二', '周三', '周四', '周五', '周六'];
        this.dWrap = dWrap;
        this.dObj = dObj;
        this.dStartInput = dStartInput;
        this.dBackInput = dBackInput;
        this.sStartDate = CheckInDate ? CheckInDate : (new Date().format('yyyy-MM-dd'));
        this.sBackDate = CheckOutDate ? CheckOutDate : new Date().addDays(1);
        var self = this;
        this.sLastDate = '';
        this.BeginDate;
        this.EndDate;
        //创建日历
        this.createCalendar();
    };
    DisplayCalendar.prototype = {
        getNextDay: function (sDate) {
            var aDate = sDate.split('-');
            return new Date(aDate[0], parseInt(aDate[1], 10) - 1, parseInt(aDate[2], 10) + 1).format('yyyy-MM-dd');
        },
        isLeap: function (year) {
            //是否为闰年
            return (year % 100 == 0 ? res = (year % 400 == 0 ? 1 : 0) : res = (year % 4 == 0 ? 1 : 0));
        },
        createCalendar: function () {
            var self = this,
			sHtml = '';
            //创建日历
            for (var i = 0; i < 2; i++) {
                sHtml += this.createSingleCalendar(i);
            }
            this.dWrap.html(sHtml);
            //绑定事件
            //this.bindEvents();
            //确定最后一个日期
            this.sLastDate = $('td[nextdate]').last().attr('date');
            //创建后设置默认时间
            var nIndex = $('[date="' + this.sStartDate + '"]').index();

            this.resetCalendar();

            self = this;
        },
        createSingleCalendar: function (n) {
            var aHtml = [], //存储节点
			currentDate = this.ServerDate ? new Date(Date.ParseString(this.ServerDate)) : new Date(), //当前Date对象
			yearNow = currentDate.getFullYear(), //当前年份
			monthNow = currentDate.getMonth() + n; //当前月份
            dateNow = currentDate.format('yyyy-MM-dd'); //今天的日期
            nextDate = this.getNextDay(dateNow), //明天的日期
			next2Date = this.getNextDay(nextDate), //明天的日期
			firstdayInMonth = new Date(yearNow, monthNow, 1), //当月第一天Date
			yearNow = firstdayInMonth.getFullYear(), //当月第一天所在年份
			firstday = firstdayInMonth.getDay(), //当月第一天星期几
			monthNow = firstdayInMonth.getMonth(), //当月第一天月份
			monthDays = new Array(31, 28 + this.isLeap(yearNow), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31); //各月份的总天数
            if (this.EndDate && this.EndDate.getMonth() < monthNow) return '';
            column = Math.ceil((monthDays[monthNow] + firstday) / 7); //表格所需要行数
            aHtml.push('<div class=low_calendar>');
            aHtml.push('<h1>' + yearNow + '年' + (monthNow + 1) + '月</h1>'); //月份加1，因为mnow变量从0开始
            aHtml.push('<div class=calGrid>');
            aHtml.push('<table><tr><th class="week_day">' + this.aWeek[0] + '</th><th>' + this.aWeek[1] + '</th><th>' + this.aWeek[2] + '</th><th>' + this.aWeek[3] + '</th><th>' + this.aWeek[4] + '</th><th>' + this.aWeek[5] + '</th><th class="week_day">' + this.aWeek[6] + '</th></tr>');
            for (var i = 0; i < column; i++) { //表格的行
                aHtml.push("<tr>");
                for (var k = 0; k < 7; k++) { //表格每行的单元格
                    var oDate,
					sDate,
					sNextDate,
					index = i * 7 + k, //每个单元格的自然序列号
					date_number = index - firstday + 1; //计算日期

                    if (date_number <= 0 || date_number > monthDays[monthNow]) {
                        //过滤无效日期（小于等于零的、大于月总天数的），即无数字的
                        date_number = "&nbsp;";
                        sDate = null;
                    } else {
                        oDate = new Date(yearNow, monthNow, date_number);
                        //获取当前的日期
                        sDate = oDate.format('yyyy-MM-dd');
                        //获取明天的日期
                        sNextDate = new Date(yearNow, monthNow, date_number + 1).format('yyyy-MM-dd');
                    }
                    //打印日期
                    if (sDate) {
                        var s = '<td nextdate=' + sNextDate + ' date=' + sDate + ' ',
						sClass = '',
						cDate = new ChineseCalendar(oDate).solarDay2(), //获取农历日期
						aDate = sDate.split('-'),
						complete = true;

                        //周六、日
                        if (k == 0 || k == 6) {
                            sClass = ' week_day';
                        }

                        //设置60天之内的日期可以选择，酒店入住日期设置为59天
                        var maxDay = 60;
                        if (this.dObj == "sInDate") {
                            maxDay = 59;
                        }

                        if (dateNow > sDate || this.BeginDate && this.EndDate <= oDate || getTimeDiff(dateNow, sDate) > maxDay) {
                            //已经过去的日期，即打灰的
                            sClass = ' disable';
                        }

                        //酒店离店日期的可选日期从入住日期开始算
                        if (this.dObj == "sOutDate") {
                            if (sDate <= this.sStartDate) {
                                sClass = ' disable';
                            }
                        }

                        //农历节日
                        if (this.chineseHoliday[cDate]) {
                            sClass += ' holiday';
                            s += 'class="' + sClass + '">' + this.chineseHoliday[cDate] + '</td>';
                            complete = false;
                        }
                        //普通节日
                        else if (this.holiday[aDate[1] + aDate[2]]) {
                            sClass += ' holiday';
                            s += 'class="' + sClass + '">' + this.holiday[aDate[1] + aDate[2]] + '</td>';
                            complete = false;
                        }
                        //今天
                        if (complete && dateNow == sDate) {
                            sClass += ' today';
                            s += 'class="' + sClass + '">今天</td>';
                            complete = false;
                        }

                        //如果不是今天明天或者节日
                        if (complete) {
                            s += 'class="' + sClass + '">' + date_number + '</td>';
                        }
                        aHtml.push(s);
                    } else {
                        aHtml.push("<td class='null'>" + date_number + "</td>");
                    }
                }
                aHtml.push("</tr>"); //表格的行结束
            }
            aHtml.push("</table>"); //表格结束
            aHtml.push('</div>');
            aHtml.push('</div>');
            return aHtml.join('');
        },
        resetCalendar: function () {
            //清除之前所选
            this.dWrap.find('.curr').removeClass('curr');
            this.dWrap.find('.curr span').remove();
            //把已经选择的日期高亮
            if (this.dBackInput) {
                var dStartDate = $('[date="' + this.sStartDate + '"]'), dEndDate = $('[date="' + this.sBackDate + '"]');
                dStartDate.addClass('curr');
                dStartDate.append('<span>入住</span>');
                dEndDate.addClass('curr');
                dEndDate.append('<span>离店</span>');
                var datebox;
                $('.low_calendar').removeAttr('id');
                if (dStartDate.length) {
                    datebox = dStartDate.closest('.low_calendar');
                    datebox.attr('id', 'datestarttag');
                } else if (dEndDate.length) {
                    datebox = dStartDate.closest('.low_calendar');
                    datebox.attr('id', 'datestarttag');

                }

            } else {
                var dStartDate = $('[date="' + this.sStartDate + '"]');
                dStartDate.addClass('curr');
                var datebox;
                $('.low_calendar').removeAttr('id');
                if (dStartDate.length) {
                    datebox = dStartDate.closest('.low_calendar');
                    datebox.attr('id', 'datestarttag');
                }
            }

            var datebox = $('#datestarttag');
//            if (datebox.length) {
//                this.startDatePos = getPosition(datebox[0]).top;
//                $('html,body').animate({ scrollTop: this.startDatePos }, 500);
//            }

        },
        infoMaker: function (sDate, nIndex) {
            if (sDate) {
                var sHtml = $('[date="' + sDate + '"]').html(),
			aDate = sDate.split('-'),
			nIndex = nIndex == 7 ? 0 : nIndex,
			oJson = {};
                oJson['year'] = aDate[0];
                oJson['month'] = parseInt(aDate[1], 10);
                oJson['date'] = parseInt(aDate[2], 10);
                oJson['sWeek'] = this.aWeek[nIndex];
                oJson['des'] = isNaN(parseInt(sHtml, 10)) ? sHtml : '';
                oJson['full'] = sDate;
                return oJson;
            }
        }
    }

    //获得元素距文档顶部的绝对位置
    function getPosition(element) {
        var top = 0, left = 0;
        if (element) {
            do {
                top += element.offsetTop;
                left += element.offsetLeft;
            } while (element = element.offsetParent);
        }
        return { top: top, left: left };
    }
})();