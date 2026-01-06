<<<<<<< HEAD
<<<<<<< HEAD
=======
[README.md](https://github.com/user-attachments/files/24400710/README.md)
>>>>>>> 820eda16f8da68b6e45519d41d624c25c7078e68
=======
[README.md](https://github.com/user-attachments/files/24400710/README.md)
>>>>>>> 820eda16f8da68b6e45519d41d624c25c7078e68
# Cat Printer Label Editor

Eine Windows-Desktop-Anwendung zum Erstellen und Drucken von Etiketten auf MX10 Thermodruckern über Bluetooth.

## 🖨️ Features

- **Bluetooth-Verbindung** zu MX10 Thermodruckern
- **Echtzeit-Vorschau** des Druckbilds in Schwarz-Weiß
- **Flexible Textpositionierung** per Drag & Drop
- **Vertikale und horizontale Ausrichtung**
- **Freie Schriftartwahl** aus allen installierten Windows-Schriftarten
- **Einstellbare Schriftgröße**
- **Papiervorschub-Steuerung** (vorwärts und rückwärts)
- **Automatisches Speichern** aller Einstellungen
- **Optimierte Ausgabe** - nur bedruckte Bereiche werden gesendet

## 📋 Voraussetzungen

- Windows 10/11
- .NET 6.0 oder höher
- MX10 Thermaldrucker (Bluetooth)
- Visual Studio 2022 (zum Kompilieren)

## 🔧 Abhängigkeiten

- **InTheHand.BluetoothLE** - Bluetooth-Kommunikation
- **System.Drawing.Common** - Bildverarbeitung

## 📦 Installation

### Option 1: Von den Releases herunterladen
1. Gehe zu [Releases](../../releases)
2. Lade die neueste Version herunter
3. Entpacke das ZIP-Archiv
4. Starte `CatPrinter.exe`

### Option 2: Aus dem Quellcode kompilieren
1. Repository klonen:
   ```bash
   git clone https://github.com/IhrUsername/cat-printer-label-editor.git
   cd cat-printer-label-editor
   ```

2. Projekt in Visual Studio 2022 öffnen

3. NuGet-Pakete wiederherstellen:
   ```bash
   dotnet restore
   ```

4. Projekt kompilieren:
   ```bash
   dotnet build
   ```

## 🚀 Verwendung

### Erste Schritte

1. **Drucker verbinden**
   - Klicke auf den "Verbinden"-Button
   - Die App sucht automatisch nach MX10-Druckern in der Nähe
   - Bei erfolgreicher Verbindung wird die MAC-Adresse gespeichert

2. **Text eingeben**
   - Gib deinen Text in das Textfeld ein
   - Die Vorschau wird automatisch aktualisiert

3. **Schriftart anpassen**
   - Wähle eine Schriftart aus der Dropdown-Liste
   - Stelle die gewünschte Schriftgröße ein
   - Optional: Aktiviere "Vertikal" für 90° gedrehten Text

4. **Text positionieren**
   - Klicke und ziehe den Text in der Vorschau
   - Die Position wird automatisch gespeichert

5. **Drucken**
   - Klicke auf "Print"
   - Der Drucker gibt nur die bedruckten Bereiche aus

### Erweiterte Funktionen

**Papiervorschub:**
- Nutze den vertikalen Schieberegler rechts
- Positive Werte = Vorwärts
- Negative Werte = Rückwärts (Retract)

**Bild speichern:**
- Klicke auf "Speichern" um die Vorschau als BMP-Datei zu exportieren

**Einstellungen werden automatisch gespeichert:**
- Schriftart und -größe
- Textinhalt
- Textposition
- Ausrichtung (horizontal/vertikal)
- Drucker MAC-Adresse

## 🎨 Technische Details

### Druckauflösung
- Breite: 384 Pixel (fest)
- Höhe: Variabel, automatisch an Inhalt angepasst

### Bildverarbeitung
- 1-Bit-Schwarz-Weiß-Konvertierung mit einstellbarem Schwellenwert
- Automatisches Zuschneiden von leerem Weißraum
- Optimierte Bitmap-Verarbeitung mit `LockBits` für Performance

### Bluetooth-Protokoll
- Service UUID: 0xAE30
- Charakteristik UUID: 0xAE01
- Unterstützt MX10 Thermal Printer Protokoll
- CRC8-Checksumme für Datenintegrität

## 📁 Projektstruktur

```
CatPrinter/
├── Form1.cs                 # Hauptformular und UI-Logik
├── Form1.Designer.cs        # Designer-generierter Code
├── MX10_inthehand.cs        # Drucker-Treiber (Bluetooth)
├── Program.cs               # Einstiegspunkt
├── Settings.Designer.cs     # Einstellungen (auto-generiert)
├── Settings.settings        # Einstellungs-Konfiguration
└── README.md               # Diese Datei
```

## 🐛 Bekannte Einschränkungen

- Nur MX10 Thermaldrucker werden unterstützt
- Bluetooth muss auf dem PC aktiviert sein
- Nur Text wird unterstützt (keine Bilder oder QR-Codes)
- Windows-Only (nutzt System.Drawing und Windows Forms)

## 🔮 Geplante Features

- [ ] Mehrsprachige Benutzeroberfläche
- [ ] QR-Code-Generierung
- [ ] Barcode-Unterstützung
- [ ] Bild-Import und -Druck
- [ ] Vorlagen-System
- [ ] Export als PDF

## 🤝 Mitwirken

Beiträge sind willkommen! Bitte:

1. Forke das Repository
2. Erstelle einen Feature-Branch (`git checkout -b feature/AmazingFeature`)
3. Committe deine Änderungen (`git commit -m 'Add some AmazingFeature'`)
4. Pushe zum Branch (`git push origin feature/AmazingFeature`)
5. Öffne einen Pull Request

## 📝 Lizenz

Dieses Projekt ist unter der MIT-Lizenz lizenziert - siehe [LICENSE](LICENSE) Datei für Details.

## 👏 Danksagungen

- **InTheHand.BluetoothLE** für die plattformübergreifende Bluetooth-Bibliothek
- MX10 Drucker-Community für Protokoll-Dokumentation
- Alle Mitwirkenden und Tester

## 📧 Kontakt

Bei Fragen oder Problemen öffne bitte ein [Issue](../../issues) auf GitHub.

---

<<<<<<< HEAD
<<<<<<< HEAD
**Hinweis:** Dieses Projekt ist nicht offiziell mit den Herstellern von MX10-Druckern verbunden oder von ihnen unterstützt.
=======
**Hinweis:** Dieses Projekt ist nicht offiziell mit den Herstellern von MX10-Druckern verbunden oder von ihnen unterstützt.
>>>>>>> 820eda16f8da68b6e45519d41d624c25c7078e68
=======
**Hinweis:** Dieses Projekt ist nicht offiziell mit den Herstellern von MX10-Druckern verbunden oder von ihnen unterstützt.
>>>>>>> 820eda16f8da68b6e45519d41d624c25c7078e68
