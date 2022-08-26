using System.ComponentModel;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniApplicationPathWindow
{
    internal sealed class ApplicationPathWindow : EditorWindow
    {
        private Vector2 m_scrollPos;

        [MenuItem( "Window/Kogane/Application Path" )]
        public static void Init()
        {
            GetWindow<ApplicationPathWindow>();
        }

        private void OnGUI()
        {
            m_scrollPos = EditorGUILayout.BeginScrollView( m_scrollPos );

            EditorGUILayout.LabelField( "Application", EditorStyles.boldLabel );

            var drawer = new Drawer( 224 );

            drawer.Draw( "dataPath", Application.dataPath );
            drawer.Draw( "consoleLogPath", Application.consoleLogPath );
            drawer.Draw( "persistentDataPath", Application.persistentDataPath );
            drawer.Draw( "streamingAssetsPath", Application.streamingAssetsPath );
            drawer.Draw( "temporaryCachePath", Application.temporaryCachePath );

            EditorGUILayout.Space();
            EditorGUILayout.LabelField( "EditorApplication", EditorStyles.boldLabel );

            drawer.Draw( "applicationContentsPath", EditorApplication.applicationContentsPath );
            drawer.Draw( "applicationPath", EditorApplication.applicationPath );

            EditorGUILayout.Space();
            EditorGUILayout.LabelField( "InternalEditorUtility", EditorStyles.boldLabel );

            drawer.Draw( "unityPreferencesFolder", InternalEditorUtility.unityPreferencesFolder );
            drawer.Draw( "GetCrashReportFolder", InternalEditorUtility.GetCrashReportFolder() );
            drawer.Draw( "GetEditorAssemblyPath", InternalEditorUtility.GetEditorAssemblyPath() );
            drawer.Draw( "GetEngineAssemblyPath", InternalEditorUtility.GetEngineAssemblyPath() );
            drawer.Draw( "GetEngineCoreModuleAssemblyPath", InternalEditorUtility.GetEngineCoreModuleAssemblyPath() );

            EditorGUILayout.EndScrollView();
        }

        private sealed class Drawer
        {
            private readonly int m_width;

            public Drawer( int width ) => m_width = width;

            public void Draw( string label, string text )
            {
                using ( new EditorGUILayout.HorizontalScope() )
                {
                    EditorGUILayout.LabelField( label, GUILayout.Width( m_width ) );

                    if ( GUILayout.Button( "Copy", GUILayout.Width( 40 ) ) )
                    {
                        EditorGUIUtility.systemCopyBuffer = text;
                    }

                    if ( GUILayout.Button( "Open", GUILayout.Width( 40 ) ) )
                    {
                        var extension = Path.GetExtension( text );
                        var isFolder  = string.IsNullOrWhiteSpace( extension );
                        var path      = isFolder ? text : Path.GetDirectoryName( text ).Replace( "\\", "/" );

                        try
                        {
                            System.Diagnostics.Process.Start( path );
                        }
                        catch ( Win32Exception e )
                        {
                            Debug.LogError( e.Message );
                        }
                    }

                    EditorGUILayout.TextField( text );
                }
            }
        }
    }
}