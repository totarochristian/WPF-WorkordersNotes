using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EvernoteClone.View
{
    /// <summary>
    /// Interaction logic for NotesWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        public NotesWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            //Close the current application
            Application.Current.Shutdown();
        }

        private async void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            //Define subscription key and region
            string region = "northeurope";
            string key = "1e7b37eaa2d24dae9b85b78c7b9135a7";
            //Retrieve the speech configuration using the subscription key and the region
            var speechConfig = SpeechConfig.FromSubscription(key, region);
            //Initialize the audio configuration using the default microphone input
            using(var audioConfig = AudioConfig.FromDefaultMicrophoneInput())
            {
                //Initialize a speech recognizer using the speech configuration, the language and the default microphone audio configuration
                using (var recognizer = new SpeechRecognizer(speechConfig, "it-IT", audioConfig))
                {
                    //Start the recognize of the input for max 30 second or unless the user stop to talk and then return a result
                    var result = await recognizer.RecognizeOnceAsync();
                    //Add the text recognized in the content rich text box as a new run of a new paragraph
                    contentRichTextBox.Document.Blocks.Add(new Paragraph(new Run(result.Text)));
                }
            } 
        }

        private void contentRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Retrieve the number of characters conained in the content rich text box
            int ammountCharacters = (new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd)).Text.Length;
            //Update the status text block adding the length of the text in the rich text box
            statusTextBlock.Text = $"Document length: {ammountCharacters} characters";
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            //IsChecked is a bool?, using ?? will evaluate the value of the property and if is null, set false
            bool isButtonCheched = (sender as ToggleButton).IsChecked ?? false;
            if(isButtonCheched)
                //Set the font weight of the selected text to bold
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                //Set the font weight of the selected text to normal
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //Retrieve the font weight property of the selected text in the content rich text box
            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            //Check if the selected weight is bold, if true, check the bold toggle button
            boldButton.IsChecked = selectedWeight.Equals(FontWeights.Bold);
        }
    }
}
