   M S B A M L   `   `   `  ����D  @UcMyVoice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:UcMyVoice	UcMyVoice  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089c _Microsoft.Expression.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35   	DoubleArc  �� 
TargetType    UcMyVoice.Loading �Name%  Microsoft.Expression.Shapes.Arc  Name   ArcThickness   ArcThicknessUnit   EndAngle J� Opacity J� RenderTransformOrigin   
StartAngle	 �� Stretch'  !System.Windows.VisualStateManager
   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard    
SpeedRatio g� 
SpeedRatio g� RepeatBehavior �� 
TargetName �� TargetProperty O� KeyTime O� Value�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      ZUIclr-namespace:UcMyVoice  5      <ec5http://schemas.microsoft.com/expression/2010/controls  5      =ed4http://schemas.microsoft.com/expression/2010/drawing  5      %�  &	        �� 5      "    6(   .R�5   
   � $ 	PART_Root��6   #��z��6"   .+�5	        $ PART_OutCircle��6   $ 5Q�6-   $ Pixel=�6>   $	 360Q�6W   #��z��5
      $ 1Q�5      $ 0.5,0.5&�6!   $ 90Q�6A   $
	 None=�6Q   z�5      �� 5      5        5      $ PART_InnerCircle��6   $��5q�6/   $ 5Q�6:   $ Pixel=�6K   $	 150Q�5      #��z��5      $	 0.4Q�5      $ 0.5,0.5&�6#   $	 -60Q�6C   $
	 None=�6T   z�5      �� 5      5      
 5        5      ActiveStates��. 5        Inactive��6"     5      
Active��. 5      �� # z 6%   .��5      U� $ Forever��6<   $ PART_OutCircle��6U   $9 3(UIElement.RenderTransform).(RotateTransform.Angle)�6|   .\�5   "   �� $ 	0:0:0.000��67   $ 0Q�6K   �� 5    "   $ 	0:0:3.000��67   $	 360Q�6K   5!      U� 5"      $ Forever��6<   $ PART_InnerCircle��6U   $9 3(UIElement.RenderTransform).(RotateTransform.Angle)�6~   .\�5#   "   �� $ 	0:0:0.000��67   $ 0Q�6K   �� 5$   "   $ 	0:0:3.000��67   $
 -360Q�6K   5%      5&      5'      5(      5*      5+      5,      