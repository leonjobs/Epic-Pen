﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Epic_Pen.util;
using Brushes = System.Windows.Media.Brushes;
using DashStyle = System.Drawing.Drawing2D.DashStyle;
using Pen = System.Drawing.Pen;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Epic_Pen
{
    /// <summary>
    /// Interaction logic for toolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {

        public List<TextBox> TextBoxList;
        InkCanvas inkCanvas;
        StackPanel TextPart;
        public int mode_text = 0;

        public ToolsWindow()
        {
            TextBoxList = new List<TextBox>();
            InitializeComponent();
        }

        public void setInkCanvas(InkCanvas _inkCanvas)
        { inkCanvas = _inkCanvas; }

        public void setScrollViewer(StackPanel _TextPart)
        { TextPart = _TextPart; }



        public void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode_text = 0;
            selectedColourBorder.Background = ((Border)sender).Background;
            inkCanvas.DefaultDrawingAttributes.Color = ((SolidColorBrush)((Border)sender).Background).Color;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mode_text = 0;
            //System.Media.SystemSounds.Asterisk.Play();
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        public event EventHandler CloseButtonClick;

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            onCloseButtonClick();
        }

        void onCloseButtonClick()
        {
            if (CloseButtonClick != null)
                CloseButtonClick.Invoke(new object(), new EventArgs());
        }

        private void resetAllToolBackgrounds()
        {
            foreach (Button i in toolStackPanel.Children)
                i.Style = defaultButtonStyle;
        }

        public void cursorButton_Click(object sender, RoutedEventArgs e)
        {
            mode_text = 0;
            resetAllToolBackgrounds();
            cursorButton.Style = (Style)FindResource("highlightedButtonStyle");
        }
        public void penButton_Click(object sender, RoutedEventArgs e)
        {
            mode_text = 0;
            inkCanvas.Cursor = Cursors.Pen;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
            setBrushSize();
            resetAllToolBackgrounds();
            penButton.Style = (Style)FindResource("highlightedButtonStyle");

        }

        public void highlighterButton_Click(object sender, RoutedEventArgs e)
        {

            inkCanvas.Cursor = Cursors.Pen;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = true;
            setBrushSize();
            resetAllToolBackgrounds();
            highlighterButton.Style = (Style)FindResource("highlightedButtonStyle");

        }
        
        public void eraserButton_Click(object sender, RoutedEventArgs e)
        {
            mode_text = 0;
            inkCanvas.Cursor = Cursors.Cross;
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            setBrushSize();
            resetAllToolBackgrounds();
            eraserButton.Style = (Style)FindResource("highlightedButtonStyle");   
        }

        public delegate void TextAddedEventHandler(object sender, RoutedEventArgs e);   
        public event TextAddedEventHandler TextAddedClick;

        public void TextAddedWhenMouseUp(object sender, RoutedEventArgs e)
        {

        }

        

        public void TextButton_OnClick(object sender, RoutedEventArgs e)
        {
            mode_text = 1;
           // // //添加第一个文本框
           // TextBox tb1 = new TextBox();
           // tb1.Name = "ProjectTB";

           // tb1.Text = "第一个文本框";
           // tb1.Name = "TextBox1";
           // //tb1.HorizontalAlignment = HorizontalAlignment.Left;
           // //tb1.VerticalAlignment = VerticalAlignment.Top;
           // //tb1.Margin = new Thickness(100, 100, 0, 0);
           // TextPart.Children.Add(tb1);
           ////TextAddedPlace.Children.Add(tb1);


            inkCanvas.Cursor = Cursors.Pen; 
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
            setBrushSize();
            resetAllToolBackgrounds();
            TextButton.Style = (Style)FindResource("highlightedButtonStyle");
        }


        public void eraseAllButton_Click(object sender, RoutedEventArgs e)
        {
            mode_text = 0;
            inkCanvas.Strokes.Clear();
        }
        double penSize=3;
        private void penSizeButton_MouseDown(object sender, RoutedEventArgs e)
        {
            penSize = ((Ellipse)((Button)sender).Content).Width;
            setBrushSize();

            foreach (Button i in brushSizeStackPanel.Children)
                i.Style = defaultButtonStyle;
            ((Button)sender).Style = (Style)FindResource("highlightedButtonStyle");   
        }

        private void setBrushSize()
        {
            if (inkCanvas.Cursor == Cursors.Cross)
            {
                inkCanvas.DefaultDrawingAttributes.Width = penSize * 5;
                inkCanvas.DefaultDrawingAttributes.Height = penSize * 5;
            }
            else
            {
                inkCanvas.DefaultDrawingAttributes.Width = penSize;
                inkCanvas.DefaultDrawingAttributes.Height = penSize;
            }
        }

        private void clickThroughCheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if ((bool)hideInkCheckBox.IsChecked)
            {
                //toolsDockPanel.Height = 0;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = toolsDockPanelDefaultHeight;
                doubleAnimation.To = 0;
                doubleAnimation.Duration = new Duration(new TimeSpan(0,0,0,0,200));
                ExponentialEase expoEase = new ExponentialEase();
                expoEase.Exponent = 7;
                doubleAnimation.EasingFunction = expoEase;
                //Storyboard.SetTargetName(doubleAnimation, toolsDockPanel.Name);
                Storyboard.SetTarget(doubleAnimation, toolsDockPanel);
                Rectangle rect = new Rectangle();
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(DockPanel.HeightProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }
            else
            {
                //toolsDockPanel.Height = double.NaN;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = 0;
                doubleAnimation.To = toolsDockPanelDefaultHeight;
                doubleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0,0, 200));
                ExponentialEase expoEase = new ExponentialEase();
                expoEase.Exponent = 7;
                doubleAnimation.EasingFunction = expoEase;
                //Storyboard.SetTargetName(doubleAnimation, toolsDockPanel.Name);
                Storyboard.SetTarget(doubleAnimation, toolsDockPanel);
                Rectangle rect = new Rectangle();
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(DockPanel.HeightProperty));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(doubleAnimation);
                storyboard.Begin();
            }

        }
        Style defaultButtonStyle;
        double toolsDockPanelDefaultHeight;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            toolsDockPanel.Height = toolsDockPanel.ActualHeight;
            toolsDockPanelDefaultHeight = toolsDockPanel.Height;
            Height = ActualHeight;
            SizeToContent = System.Windows.SizeToContent.Manual;
            defaultButtonStyle = eraseAllButton.Style;

            /**
             * 设置默认的画笔为红色
             */
            Border border = new Border();
            border.Background = Brushes.Red;
            Border_MouseDown(border, new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, TimeStamp.Current, MouseButton.Left));
        }

        /**
         * 绘制一个箭头，记着用完dispose掉
         */
        public Pen ArrowPen()
        {
            Pen arrawPen = new Pen(System.Drawing.Brushes.Red);
            arrawPen.DashStyle = DashStyle.Dash;
            arrawPen.EndCap = LineCap.ArrowAnchor;
            return arrawPen;
        }

        private void ArrowButton_OnClick(object sender, RoutedEventArgs e)
        {
            inkCanvas.Cursor = Cursors.ScrollAll;
            inkCanvas.EditingMode = InkCanvasEditingMode.GestureOnly;
            inkCanvas.DefaultDrawingAttributes.IsHighlighter = false;
            setBrushSize();
            resetAllToolBackgrounds();
            ArrowButton.Style = (Style)FindResource("highlightedButtonStyle");
        }
        private void TextDelete_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxList.ForEach(ChangeTextBox);
        }
        private void ChangeTextBox(TextBox a)
        {
            a.Visibility = Visibility.Hidden;
        }

    }
}
