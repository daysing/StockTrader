做访问服务器的接口

获取实时行情接口00:
ws://[localhost:port]/websock/stockmarket

获取策略列表接口: 10
[localhost:port]/api/v1/get_strategy_list

获取我的策略列表: 11
[localhost:port]/api/v1/get_mystrategy_list/

获取基金信息接口（净值、到期时间）
[localhost:port]/api/v1/get_fund_data/?list=sz150023,sz150024&field=

获取5分钟分时数据:20
[localhost:port]/api/v1/get_fenshi_data/5/?list=sz150023&start_date=2015-01-01&end_date=2015-01-02

获取15分钟分时数据:21
[localhost:port]/api/v1/get_fenshi_data/15/?list=sz150023&start_date=2015-01-01&end_date=2015-01-02

获取30分钟分时数据:22
[localhost:port]/api/v1/get_fenshi_data/30/?list=sz150023&start_date=2015-01-01&end_date=2015-01-02

获取60分钟分时数据:23
[localhost:port]/api/v1/get_fenshi_data/60/?list=sz150023&start_date=2015-01-01&end_date=2015-01-02

