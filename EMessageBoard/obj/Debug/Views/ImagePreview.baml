   M S B A M L   `   `   `  ����  EMessageBoard&     EMessageBoard.Views.ImagePreview   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5        $
��1218��5   8   $	��592��6E     � 	Resources  5         InitialMatrixTransform u� Matrix � OffsetX � OffsetY
 � M11
 � M22 
 FlipIn �� TargetProperty W� From	 W� To	 g� Duration
 g� DecelerationRatio  FlipOut g� AccelerationRatio  appbar_close J� Clip �� Left
 �� Top �� Stretch��%�  &	        &	 f     &	 �    &	     u� 5   
    5      � 5      $ 0Q�6   $ 0Q�6%   $ 1Q�61   $ 1Q�69   5      �� 5   
   .��5      W� $, &RenderTransform.(SkewTransform.AngleX)�6   $
 -100X�6a   $ 0X�6m   $	 0:0:.25D�6t   $
 .9Q�6�   W� 5      $, &RenderTransform.(SkewTransform.AngleY)�6   $
 -100X�6a   $ 0X�6m   $	 0:0:.25D�6t   $
 .9Q�6�   W� 5      $	 0:0:.25D�6   $ Opacity�61   $ 0X�6U   $ 1X�6^   5      �� 5   
   .��5      W� $, &RenderTransform.(SkewTransform.AngleX)�6   $	 100X�6a   $	 0:0:.25D�6j   $ .9Q�6}   W� 5      $, &RenderTransform.(SkewTransform.AngleY)�6   $	 100X�6a   $	 0:0:.25D�6j   $ .9Q�6}   W� 5      $	 0:0:.75D�6   $ Opacity�61   $ 0X�6U   5      �� 5   
   $��76��6&   $��76��61   4 ��        � ��    � �� ���     ���        .+�5      F� $��31.6666��6   $��31.6667��6#   $ 22.1666��64   $ 22.1667��6J   $
 Fill=�6_   
���   �|��������<R��C�Z��@��=R��m5�����O���C��m5��Ѥ���n5��C0P������n5�H<R�����Z���CѬ<R\�������<R6{   5      5      .��5      � -   5         �Name$ mainGrid��5      
������� J� IsManipulationEnabled .  J� Opacity$ 1Q�6M   z�5    
   �� 5!       �� AngleX$ 0Q�6    �� AngleY$ 0Q�6'    �� CenterX$	 350Q�62    �� CenterY$	 285Q�6@   .+�5#   
   �� -   5         ��Name$ 	MainImage��5#      $>��8/EMessageBoard;component/Resources/images/background.png��6#   �� 5$   
   -   5         ��Name$ CloseBtn��5$      $��50��6#   $��50��6.   $	��Top=�6:   
���   �$��Right=�6e   $��	0,20,20,0q�6�    J� OpacityMask 5%      !� 5&       j� Stretch$
 Fill=�6    !� Visual# �  5'      5)      5*      5+      