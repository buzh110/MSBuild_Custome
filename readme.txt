添加管理员权限

1、在[Setup]节点添加 PrivilegesRequired=admin

2、进入安装目录，找到文件SetupLdr.e32，这是一个二进制配置文件，需要用到ResHacker.exe这个工具修改

　  找到<requestedExecutionLevel level="asInvoker" uiAccess="false"/></requestedPrivileges>，

     修改为<requestedExecutionLevel level="requireAdministrator" uiAccess="false"/></requestedPrivileges>

 

删除管理员权限

1、在[Setup]节点添加 PrivilegesRequired=none 或者 PrivilegesRequired=lowest

2、在[Setup]节点修改 DefaultDirName（安装路径）的值，默认可能是DefaultDirName={pf}\{#MyAppName}，

　 不要带{pf}, {win}, {sys}这些变量值，比如设置成DefaultDirName={localappdata}\{#MyAppName}