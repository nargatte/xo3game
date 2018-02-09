using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MathNet.Numerics.LinearAlgebra;
using OX3DGame.GraphicsEngine;
using SharpGL;

namespace OX3DGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenGL gl;
        private RenderManager RenderManager;

        public MainWindow()
        {
            InitializeComponent();
            InitializeMenuItems();
            InitializeOpenGl();
        }

        private void InitializeMenuItems()
        {
            Action actionForShaders = () =>
            {
                if ((MenuItemShadingMode.Items[1] as MenuItem).IsChecked)
                {
                    if ((MenuItemShadingModel.Items[1] as MenuItem).IsChecked)
                        RenderManager.ChangeShader(new GouraudBlinnShader(gl));
                    else
                        RenderManager.ChangeShader(new GouraudPhongShader(gl));
                }
                else
                {
                    if ((MenuItemShadingModel.Items[1] as MenuItem).IsChecked)
                        RenderManager.ChangeShader(new PhongBlinnShader(gl));
                    else
                        RenderManager.ChangeShader(new PhongPhongShader(gl));
                }
            };

            SetMenuItemsAsCheckableGroup(MenuItemAnimation.Items, () =>
            {
                AnimationType at = AnimationType.None;
                if((MenuItemAnimation.Items[1] as MenuItem).IsChecked) at = AnimationType.Tracking;
                if((MenuItemAnimation.Items[2] as MenuItem).IsChecked) at = AnimationType.Observing;
                RenderManager.AnimationType = at;
            });
            SetMenuItemsAsCheckableGroup(MenuItemShadingMode.Items, actionForShaders);
            SetMenuItemsAsCheckableGroup(MenuItemShadingModel.Items, actionForShaders);
            SetMenuItemsAsCheckableGroup(MenuItemDisplayMode.Items, () =>
            {
                RenderManager.SetLineMode(!(MenuItemDisplayMode.Items[0] as MenuItem).IsChecked);
            });
        }

        private void InitializeOpenGl()
        {
            gl = openGLControl.OpenGL;
            RenderManager = new RenderManager(gl);
        }

        private void SetMenuItemsAsCheckableGroup(ItemCollection ic, Action changeInvoke)
        {
            foreach (MenuItem menuItem in ic)
            {
                menuItem.IsCheckable = true;
                menuItem.Checked += (sender, args) => 
                {
                    foreach (MenuItem menuItem2 in ic)
                    {
                        if(menuItem2 == menuItem) continue;
                        menuItem2.IsChecked = false;
                    }
                    changeInvoke();
                };
                menuItem.Click += (sender, args) =>
                {
                    if (menuItem.IsChecked == false)
                        menuItem.IsChecked = true;
                };
            }
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            RenderManager.Initialize();
        }

        private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            RenderManager.Resize((int) openGLControl.ActualWidth, (int) openGLControl.ActualHeight);
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            RenderManager.Draw();
        }

        private void MenuItem_OpenGLVersion(object sender, RoutedEventArgs e)
        {
            string vendor = gl.GetString(OpenGL.GL_VENDOR);
            string render = gl.GetString(OpenGL.GL_RENDERER);
            string version = gl.GetString(OpenGL.GL_VERSION);
            string shaderLanguageVersion = gl.GetString(OpenGL.GL_SHADING_LANGUAGE_VERSION);
            MessageBox.Show("Version: " + version + "\n" + "Vendor: " + vendor + "\n" + "Render: " + render + "\n" +
                            "Shading Language Version: " + shaderLanguageVersion, "OpenGL Info");
        }

        private void OpenGLControl_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(openGLControl);
            float x = (float) (2f * p.X / openGLControl.ActualWidth - 1);
            float y = -(float) (2f * p.Y / openGLControl.ActualHeight - 1);
            RenderManager.ClickOn(x, y);
        }

        private void OpenGLControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(openGLControl);
            float x = (float)(2f * p.X / openGLControl.ActualWidth - 1);
            float y = -(float)(2f * p.Y / openGLControl.ActualHeight - 1);
            RenderManager.MouseMove(x, y);
        }

        private void MenuItem_OnClickExit(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            RenderManager.Dispose();
        }

        private void MenuItem_OnClickNewGame(object sender, RoutedEventArgs e)
        {
            RenderManager.ResetGame();
        }
    }
}
