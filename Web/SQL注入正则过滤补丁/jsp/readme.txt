1.SQLFilter.javaΪ���˵�Դ�ļ��������ж�SQL���˵Ĺؼ��ֽ����޸ġ������Լ���ĿԴ���н��б��롣��·����һ�µ�ʱ�����޸İ�����
2.SQLFilter.classΪ�������ļ�����������޸ģ��޸ĵĻ���SQLFilter.java�����޸ģ�������ĿwebĿ¼/WEB-INF/classes/com/filter/�¡�
3.�޸�web.xml���������´��롣
    <filter>
        <filter-name>SQLFilter</filter-name>
        <filter-class>com.filter.SQLFilter</filter-class>
    </filter>
    <filter-mapping>
        <filter-name>SQLFilter</filter-name>
        <url-pattern>/*</url-pattern><!-- ������������е����󶼽��й��� -->
    </filter-mapping>
    <welcome-file-list>
