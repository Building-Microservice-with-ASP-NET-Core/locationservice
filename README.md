
[![wercker status](https://app.wercker.com/status/1775588c38ae3696bb1c2a3e66bb2065/s/master "wercker status")](https://app.wercker.com/project/byKey/1775588c38ae3696bb1c2a3e66bb2065)

# Location Service

用于保留团队成员历史位置的微服务。

在运行此分支的代码之前，需要正确设置这两个环境变量：

* TRANSIENT (boolean 类型)
* POSTGRES_CSTR (postgres 连接字符串)

docker run -p 5432:5432 --name some-postgres -e POSTGRES_PASSWORD=inteword -e POSTGRES_USER=integrator -e POSTGRES_DB=locationservice postgres

docker run -it --rm --link some-postgres:postgres postgres psql -h postgres -U integrator -d locationservice