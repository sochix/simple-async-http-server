�����: ��������� ����, �.����, 2013 �.

# ������� ������ �� C# ��� ������������ WCF � ASP.NET

������ ������� �� ����� C# � ������������ ����������� ��������� ��������, ����� �������� � �� xml � SQLite.
������� � �������������� ����������� TDD. ��� ����������� ����������� ����� NLog.

����� �������� ��� ������ ������� ��� ���� ����������, ������� � ��� �� ������� ��������� ��������� ������ � �����������. 

## ������ ������

� ������ ������� ������� ������� ������ ������.
��� ��� ������ ��������� ������ GET �� <�����_�������>/proxy/?url=<�����_�����> 
��� <�����_�������> �� ��������� 127.0.0.1:8888
<�����_�����> ����� � ������� http://www.ya.ru

## �������� �����

����� �������� ��� ������ �� �������� ����� ��������� GET ������ �� ����� <�����_�������>/guestbook/
����� �������� ������ � �������� ����� ��������� POST ������ �� ����� <�����_�������>/guestbook/?user=<username>&?message=<message>

# ��������� � ���������
## ������

������ ������������ ����� ���������� ����������. ��� ������ ������, ���������
������� ������ VisualStudio 2010 AnyWayAnyDayDemo.sln �� ����������� �����. 
��� �������� ������ ���������, ��� ���������� ����� NLog � ���������� �������� �� ���������� ��� ������
� �� SQLite.

����� �������� ������ �������, ��������� ��� � ������� ������� F5 ��� ��������������� �� ����� bin.

## ���������
������� ��������� ������� ��������� � ����t server_config.xml. 
������������� ����� ���������� � ����������� ����� ServerHelpers.cs

# ������
��� ���������� ������, ���������� �������� � ��� ������ �� ������ http://www.sochix.ru/ContactForm.aspx/Create

