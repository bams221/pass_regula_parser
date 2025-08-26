using PassRegulaParser.Models;

namespace PassRegulaParser.Ui.FieldBuilders;

public class PhotoFieldBuilder(
    TableLayoutPanel mainPanel,
    DocumentEditWindow window,
    Dictionary<string, Control> fieldControls)
{
    private readonly TableLayoutPanel _mainPanel = mainPanel;
    private readonly DocumentEditWindow _window = window;
    private readonly Dictionary<string, Control> _fieldControls = fieldControls;

    public void AddPhotoField()
    {
        var rowIndex = _mainPanel.RowCount;
        _mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
        _mainPanel.RowCount++;

        var photoLabel = new Label
        {
            Text = "Фото:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 5, 10, 5)
        };

        var photoBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom
        };

        LoadPhotoIntoPictureBox(photoBox);

        _mainPanel.Controls.Add(photoLabel, 0, rowIndex);
        _mainPanel.Controls.Add(photoBox, 1, rowIndex);
        _fieldControls.Add(nameof(PassportData.PhotoBase64), photoBox);
    }

    private void LoadPhotoIntoPictureBox(PictureBox photoBox)
    {
        var photoBase64 = _window.GetPropertyValue(nameof(PassportData.PhotoBase64)) as string;
        if (!string.IsNullOrEmpty(photoBase64))
        {
            try
            {
                byte[]? imageBytes = Convert.FromBase64String(photoBase64);
                using var ms = new MemoryStream(imageBytes);
                photoBox.Image = Image.FromStream(ms);
            }
            catch
            {
                SetErrorState(photoBox, "Не удалось загрузить фото");
            }
        }
        else
        {
            SetErrorState(photoBox, "Фото отсутствует");
        }
    }

    private static void SetErrorState(PictureBox photoBox, string message)
    {
        photoBox.Image = null;
        photoBox.BackColor = Color.LightGray;
        photoBox.Paint += (sender, e) =>
        {
            using var font = new Font("Arial", 9);
            var textSize = e.Graphics.MeasureString(message, font);
            var x = (photoBox.Width - textSize.Width) / 2;
            var y = (photoBox.Height - textSize.Height) / 2;
            e.Graphics.DrawString(message, font, Brushes.Black, x, y);
        };
    }
}
