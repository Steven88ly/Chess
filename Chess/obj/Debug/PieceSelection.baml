   M S B A M L   `   `   `  ����A  =Chess, Version=1.16.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ChessChess  5   	       Chess.PieceSelection   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Chess     5   	     � Title$  	Promotion��5   	    � Icon$ Img/Icons/mainIcon.ico��6   $	��138��5	   	   $	��512��6    � WindowStartupLocation$ CenterOwner=�5
   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�6   
���   �$��1q�5      .��5      � -   5        $��10q�5   H    � RowDefinitions 5   
   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��
rookButton��5   D   $��0q�6   $��0q�6%   
�������6D   �� 5   
   -   5        $��knightButton��5   T   $��0q�6   $��0q�6%   
�������$��1��6D   6T   �� 5   
   -   5        $��bishopButton��5   T   $��0q�6   $��0q�6%   
�������$��2��6D   6T   �� 5   
   -   5        $��queenButton��5   T   $��0q�6   $��0q�6%   
�������$��3��6D   6T   5      5      