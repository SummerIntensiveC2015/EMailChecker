   M S B A M L   `   `   `  ����  Email_Notifications%    Email_Notifications.SetSettings   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  SetSettings��5   	   $	��400��6   $	��470��6*    � AllowsTransparency .  � WindowStyle$
 None=�6P    J� Opacity$
 0.95Q�6c    � ShowInTaskbar .   � Topmost . .��5      � .+�5   
   �� $
��Left=�6   $	��380��60   $��	10,10,0,0q�6=   $	��Top=�6P   $	��450��6h   .@�5      �� $��Пользователь��6   
�������.��5      � 
�������$��0,-1,0,1q�6*   .+�5	      &   System.Windows.Controls.DataGrid  -   5          Name$ DataGridUsers��5	      $
��Left=�67   $	��Top=�6R   $	��266��6j   $	��420��6w      
IsReadOnly . $��	10,40,0,0q�6�      GridLinesVisibility$
 None=�6�   
�������5
      �� 5      $)��#Активность адресов��6   $
��Left=�69   $��	10,10,0,0q�6T   $	��Top=�6g   $	��160��6   $��16(�6�   $��30��6�   �� 5      -   5        $��Правка��5      $
��Left=�6.   $��
90,311,0,0q�6I   $	��Top=�6]   $��75��6u   $��27��6�   
�������6�   �� 5      -   5        $��Удалить��5      $
��Left=�6/   $��170,311,0,0q�6J   $	��Top=�6_   $��75��6w   $��27��6�   
�������6�   �� 5      -   5        $��Добавить��5      $
��Left=�60   $��
10,311,0,0q�6K   $	��Top=�6_   $��75��6w   $��27��6�   
�������6�   5      5      �� 5      $��Система��6   
�������.��5      � 
�������.+�5      �� $%��Частота запросов��6   $
��Left=�67   $��	10,10,0,0q�6R   $	��Top=�6e   $��22(�6}   �� 5      $<��6Время активности уведомления��6   $
��Left=�6C   $��
10,152,0,0q�6^   $	��Top=�6r   $��22(�6�   �� 5      $��Один раз в��6   $
��Left=�61   $��	27,93,0,0q�6L   $	��Top=�6_   $��16(�6w   �� 5      $��
минут��6   $
��Left=�6,   $��
267,93,0,0q�6G   $	��Top=�6[   $��16(�6s   $��76��6�   �� 5      $��секунд��6   $
��Left=�6-   $��267,228,0,0q�6H   $	��Top=�6]   $��16(�6u   $��76��6�   �� 5      -   5        	 ��Name$	 buttonGlobalSettingsApply��5      $��Применить��6@   $
��Left=�6T   $��296,303,0,0q�6o   $	��Top=�6�   $	��134��6�   
���   �$��22(�6�   
�������6�   � 5      $��137,228,200,89q�6   
 � ColumnDefinitions
 5      �� 5      �� 5      $��27 �6/    � RowDefinitions 5      �� 5       $��13 �6,   �� 5!      $��13 �6,   .+�5#      �� -   5        $��NUDTextBoxSeconds��5#   "   $��0��6<   $��0��6L   $��2��6Y    �� TextAlignment$ Right=�6j   6�   �� 5$      -   5        $��NUDButtonUPSeconds��5$   '   $��1��6B   $��0��6R   $��8(�6_   $��Marlett)�6l   $��Center=�6�   $��Center=�6�   .��6�   56�   �� 5%      -   5        $��NUDButtonDownSeconds��5%   '   $��1��6D   $��1��6T   $��8(�6a   $��Marlett)�6o   $��Center=�6�   $��Center=�6�   $��13��6�   $��Bottom=�6�   .��6  66  5&      � 5(      $��137,93,200,224q�6   
 5)      �� 5*      �� 5+      $��27 �6/    5-      �� 5.      $��13 �6,   �� 5/      $��13 �6,   .+�51      �� -	   5        $��
NUDTextBox��51   "   $��0��65   $��0��6E   $��2��6R   $ Right=�6c   6�   �� 52      -
   5        $��NUDButtonUP��52   '   $��1��6;   $��0��6K   $��8(�6X   $��Marlett)�6e   $��Center=�6z   $��Center=�6�   .��6�   56�   �� 53      -   5        $��NUDButtonDown��53   '   $��1��6=   $��1��6M   $��8(�6Z   $��Marlett)�6h   $��Center=�6}   $��Center=�6�   $��13��6�   $��Bottom=�6�   .��6  66	  54      56      57      58      �� 59   
   -   5        $	 buttonClose��59      $��X��6&   $
��Left=�62   $��	425,0,0,0q�6M   $	��Top=�6`   $��45��6x   $��32��6�    J� RenderTransformOrigin$ 0.527,0.225&�6�   $��22(�6�   $
��Bold"�6�   $��Italic$�6�   $��Segoe UI Black)�6�   
�������
�������6)  5:      5;      