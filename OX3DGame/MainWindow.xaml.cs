﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            SetMenuItemsAsCheckableGroup(MenuItemAnimation.Items, () => { });
            SetMenuItemsAsCheckableGroup(MenuItemShadingMode.Items, () => { });
            SetMenuItemsAsCheckableGroup(MenuItemShadingModel.Items, () => { });
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
                menuItem.Checked += (sender, args) => { changeInvoke();
                    foreach (MenuItem menuItem2 in ic)
                    {
                        if(menuItem2 == menuItem) continue;
                        menuItem2.IsChecked = false;
                    }
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
    }
}
