using PassRegulaParser.Model;
using PassRegulaParser.Services;

namespace PassRegulaParser.Ui
{
    public class DocumentEditWindow : Form
    {
        private readonly PassportData _documentData;
        private readonly TableLayoutPanel _mainPanel;
        private readonly Dictionary<string, Control> _fieldControls = [];

        public DocumentEditWindow(PassportData passportData)
        {
            _documentData = passportData;
            InitializeWindowProperties();
            _mainPanel = CreateMainPanel();
            InitializeFields();
            InitializeSaveButton();
            Controls.Add(_mainPanel);
        }

        private void InitializeWindowProperties()
        {
            Text = "Данные отсканированного документа";
            Size = new Size(450, 650);
            MinimumSize = new Size(400, 500);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private TableLayoutPanel CreateMainPanel()
        {
            return new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(10),
                AutoScroll = true
            };
        }

        private void InitializeFields()
        {
            // Настройка столбцов
            _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            _mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            // Добавление полей документа
            AddReadOnlyField("Тип документа:", nameof(_documentData.DocumentType));
            AddEditableField("ФИО:", nameof(_documentData.FullName));
            AddEditableField("Серия/номер:", nameof(_documentData.SerialNumber));
            AddEditableField("Город рождения:", nameof(_documentData.BirthCity));
            AddEditableField("Дата рождения:", nameof(_documentData.BirthDate));
            AddEditableField("Пол:", nameof(_documentData.Gender));
            AddEditableField("Описание:", nameof(_documentData.Description), true);
            AddPhotoField();
        }

        private void AddReadOnlyField(string labelText, string propertyName)
        {
            var propertyValue = GetPropertyValue(propertyName)?.ToString() ?? "";
            var rowIndex = _mainPanel.RowCount;
            
            _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _mainPanel.RowCount++;

            var label = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 5, 10, 5)
            };

            var valueLabel = new Label
            {
                Text = propertyValue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 5, 0, 5)
            };

            _mainPanel.Controls.Add(label, 0, rowIndex);
            _mainPanel.Controls.Add(valueLabel, 1, rowIndex);
            _fieldControls.Add(propertyName, valueLabel);
        }

        private void AddEditableField(string labelText, string propertyName, bool isMultiline = false)
        {
            var propertyValue = GetPropertyValue(propertyName)?.ToString() ?? "";
            var rowIndex = _mainPanel.RowCount;
            
            _mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _mainPanel.RowCount++;

            var label = new Label
            {
                Text = labelText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 5, 10, 5)
            };

            Control inputControl;
            
            if (isMultiline)
            {
                inputControl = new TextBox
                {
                    Text = propertyValue,
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Height = 60,
                    Margin = new Padding(0, 5, 0, 5)
                };
            }
            else
            {
                inputControl = new TextBox
                {
                    Text = propertyValue,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 5, 0, 5)
                };
            }

            _mainPanel.Controls.Add(label, 0, rowIndex);
            _mainPanel.Controls.Add(inputControl, 1, rowIndex);
            _fieldControls.Add(propertyName, inputControl);
        }

        private void AddPhotoField()
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

            if (!string.IsNullOrEmpty(_documentData.PhotoBase64))
            {
                try
                {
                    byte[]? imageBytes = Convert.FromBase64String(_documentData.PhotoBase64);
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

            _mainPanel.Controls.Add(photoLabel, 0, rowIndex);
            _mainPanel.Controls.Add(photoBox, 1, rowIndex);
            _fieldControls.Add(nameof(_documentData.PhotoBase64), photoBox);
        }

        private void SetErrorState(PictureBox photoBox, string message)
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

        private void InitializeSaveButton()
        {
            var rowIndex = _mainPanel.RowCount;
            _mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            _mainPanel.RowCount++;

            var saveButton = new Button
            {
                Text = "Сохранить",
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(0, 10, 0, 0)
            };
            saveButton.Click += (sender, e) => SaveButton_Click();

            _mainPanel.SetColumnSpan(saveButton, 2);
            _mainPanel.Controls.Add(saveButton, 0, rowIndex);
        }

        private void SaveButton_Click()
        {
            UpdateDocumentDataFromControls();
            DocumentDataSaver.SaveToJson(_documentData);
            CloseWindow();
        }

        private void UpdateDocumentDataFromControls()
        {
            foreach (var field in _fieldControls)
            {
                var control = field.Value;
                var value = control is TextBox textBox ? textBox.Text :
                           control is Label label ? label.Text : 
                           null;

                if (value != null)
                {
                    SetPropertyValue(field.Key, value);
                }
            }
        }

        private object? GetPropertyValue(string propertyName)
        {
            var property = _documentData.GetType().GetProperty(propertyName.Replace("_documentData.", ""));
            return property?.GetValue(_documentData);
        }

        private void SetPropertyValue(string propertyName, object value)
        {
            var property = _documentData.GetType().GetProperty(propertyName.Replace("_documentData.", ""));
            property?.SetValue(_documentData, value);
        }

        private void CloseWindow()
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}