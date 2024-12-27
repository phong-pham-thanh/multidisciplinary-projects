using RJCP.IO.Ports;

public interface ISerialPortManager
{
    SerialPortStream GetSerialPort();
    void SetSerialPort(SerialPortStream value);
    void OpenPort(string portName);
    void ClosePort();
    bool IsPortOpen();
    bool GetIrgnore();
    void SetIrgnore(bool value);
}

public class SerialPortManager : ISerialPortManager
{
    private SerialPortStream _serialPort;
    private bool _irgnore;

    public bool GetIrgnore()
    {
        return _irgnore;
    }
    public void SetIrgnore(bool value)
    {
        _irgnore = value;
    }


    public SerialPortStream GetSerialPort()
    {
        return _serialPort;
    }
    public void SetSerialPort(SerialPortStream value)
    {
        _serialPort = value;
    }

    public void OpenPort(string portName)
    {
        if (_serialPort == null || !_serialPort.IsOpen)
        {
            _serialPort = new SerialPortStream(portName, 115200);
            _serialPort.Open();
            Console.WriteLine("Cổng nối tiếp đã được mở.");
        }
    }

    public void ClosePort()
    {
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
            _serialPort = null;
            Console.WriteLine("Cổng nối tiếp đã được đóng.");
        }
    }

    public bool IsPortOpen()
    {
        return _serialPort != null && _serialPort.IsOpen;
    }
}
