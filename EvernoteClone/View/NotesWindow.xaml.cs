using EvernoteClone.ViewModel;
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
        NotesVM viewModel;

        public NotesWindow()
        {
            InitializeComponent();

            //Initialize the viem model equals to the notes view model defined in the resources of the window
            viewModel = Resources["vm"] as NotesVM;

            //Retrieve the system font families ordered by the name (source)
            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            //Assign the font families ordered as source of font family combo box
            fontFamilyComboBox.ItemsSource = fontFamilies;

            //Define a basic list of font sizes
            List<double> fontSizes = new List<double>() { 8, 9, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 48, 64, 72 };
            //Assign the font sizes defined to the font sizes combo box
            fontSizesComboBox.ItemsSource = fontSizes;
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

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            //IsChecked is a bool?, using ?? will evaluate the value of the property and if is null, set false
            bool isButtonCheched = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonCheched)
                //Set the font style of the selected text to italic
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                //Set the font style of the selected text to italic
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            //IsChecked is a bool?, using ?? will evaluate the value of the property and if is null, set false
            bool isButtonCheched = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonCheched)
                //Set the text decoration of the selected text to underline
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                TextDecorationCollection textDecorations;
                //Try to remove the underline text decoration from the selection and out the result in the text decorations collection
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                //Set the text decoration collection remove from the underline decoration to the selected text
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //If user selected an item
            if (fontFamilyComboBox.SelectedItem != null)
            {
                //Set the font family of the selected text to the font family selected by the user in the relative combo box
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void FontSizesComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Set the font size of the selected text to the font size selected by the user in the relative combo box
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizesComboBox.SelectedItem);
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //Retrieve the font weight property of the selected text in the content rich text box
            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            //Check if the selected weight isn't unset and is bold, if true, check the bold toggle button
            boldButton.IsChecked = selectedWeight != DependencyProperty.UnsetValue && selectedWeight.Equals(FontWeights.Bold);

            //Retrieve the font style property of the selected text in the content rich text box
            var selecteItalic = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            //Check if the selected italic isn't unset and is italic, if true, check the italic toggle button
            italicButton.IsChecked = selecteItalic != DependencyProperty.UnsetValue && selecteItalic.Equals(FontStyles.Italic);

            //Retrieve the text decoration property of the selected text in the content rich text box
            var selectedUnderline = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            //Check if the selected weight isn't unset and is underline, if true, check the underline toggle button
            underlineButton.IsChecked = selectedUnderline != DependencyProperty.UnsetValue && selectedUnderline.Equals(TextDecorations.Underline);

            //Retrieve the font family property of the selected text in the content rich text box, then save as selected item of font family combo box
            var selectedFontFamily = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontFamilyComboBox.SelectedItem = selectedFontFamily;

            //Retrieve the font size property of the selected text in the content rich text box, then save in the text of font size combo box
            var selectedFontSize = contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
            fontSizesComboBox.Text = selectedFontSize.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
