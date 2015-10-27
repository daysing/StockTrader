1、配置信息（采用文件保存，维护方便）
1.1 当前使用的交易接口("dll path, clazz name")
1.1.1 当前的资金账户，交易密码，服务密码(根据券商不同)
1.2 当前使用的行情接口("dll path, clazz name")
1.2.1 刷新行情的时间，主动推送行情不需要
1.3 

读写以下内容到数据库
1、策略库（策略可重复）
ID:        主键
NAME:      名称
DESC:      说明
CLAZZ:     类名
DLL:       DLL名称
CANCEL_SPAN_TIME:  秒

2、分级基金库(一基金一记录，基金不可重复)
ID;
CODEA:
NAMEA:
NETVALUEA:
SCALEA:	拆分比例，常规：5
CONVENTION_YIELD:
CODEB:
NAMEB:
NETVALUEB:
CODEM:
NAMEM:
NETVALUEM:
CODEI:
NAMEI:
DATE:      永续，非永续就写到期时间
H_DISCOUNT:
L_DISCOUNT:
HAS_H_DISCOUNT:
HAS_L_DISCOUNT:
UPDATE_DATE:

3、

