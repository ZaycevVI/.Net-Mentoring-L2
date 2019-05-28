<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0"
                 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                 xmlns:local="urn:local" extension-element-prefixes="msxsl"
                >

  <msxsl:script language="CSharp" implements-prefix="local">
    public string dateTimeNow()
    {
    return DateTime.Now.ToString("yyyy-MM-dd");
    }
  </msxsl:script>

  <xsl:output method="html" indent="yes"/>

    <!--<xsl:template match="book">
    <xsl:for-each-group select="file" group-by="@genre">
      <xsl:value-of select="current-grouping-key()"/>
      <xsl:text>
 </xsl:text>
    </xsl:for-each-group>
    
  </xsl:template>-->

  <xsl:template match="/">

    <!-- Header -->
    <xsl:text>Текущие фонды по жанрам&#10;</xsl:text>
    <ul>

      <!-- Body -->

      <!--<table>
        <thead>
          <tr>
            <th>
              <xsl:text>Item Number</xsl:text>
            </th>
            <th>
              <xsl:text>Quantity</xsl:text>
            </th>
            <th>
              <xsl:text>Unit Price</xsl:text>
            </th>
            <th>
              <xsl:text>Total</xsl:text>
            </th>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each-group select="catalog/book" group-by="genre">
            <xsl:sort select="current-grouping-key()"/>
            <tr>
              <td>
                <xsl:value-of select="current-group()/Author"/>
              </td>
              <td>
                <xsl:value-of select="sum(current-group()/Quantity)"/>
              </td>
              <td>
                <xsl:value-of select="current-group()/UnitPrice"/>
              </td>
              <td>
                <xsl:value-of
                  select="sum(for $x in current-group() return $x/UnitPrice * $x/Quantity)"/>
              </td>
            </tr>
            <xsl:apply-templates select="current-group()"/>
          </xsl:for-each-group>
        </tbody>
      </table>-->
      
      <xsl:for-each select="//genre[not(.=preceding::genre)]">
        <li>
          <span style="font-weight:bold">
            <xsl:value-of select="."/> -
          </span>
          <xsl:value-of select="local:dateTimeNow()"/>

            <table border="1">
              <tr bgcolor="#CCCCCC">
                <td align="center">
                  <strong>Author</strong>
                </td>
                <td align="center">
                  <strong>Title</strong>
                </td>
                <td align="center">
                  <strong>Publish Date</strong>
                </td>
                <td align="center">
                  <strong>Registration Date</strong>
                </td>
              </tr>

              <xsl:variable name="stringReplace">
                <xsl:value-of select="string(.)"/>
              </xsl:variable>
              <!--<xsl:value-of select="$stringReplace"/>-->
              
              <xsl:for-each select="//catalog/book[genre=$stringReplace]">
              <tr bgcolor="#F5F5F5">
                <td>
                  <xsl:value-of select="./author"/>
                </td>
                <td>
                  <xsl:value-of select="./title"/>
                </td>
                <td>
                  <xsl:value-of select="./publish_date"/>
                </td>
                <td>
                  <xsl:value-of select="./registration_date"/>
                </td>
              </tr>
          </xsl:for-each>
            </table>

         <br>
            <xsl:value-of select="concat('Итого:', count(//catalog/book))"/>
          </br>
          
          <!--<table border="1">
            <tr bgcolor="#CCCCCC">
              <td align="center">
                <strong>Author</strong>
              </td>
              <td align="center">
                <strong>Title</strong>
              </td>
              <td align="center">
                <strong>Publish Date</strong>
              </td>
              <td align="center">
                <strong>Registration Date</strong>
              </td>
            </tr>
            <tr bgcolor="#F5F5F5">
              <td>
                <xsl:value-of select="//book/author"/>
              </td>
              <td>
              <xsl:value-of select="//title"/>
              </td>
              <td>
                <xsl:value-of select="//publish_date"/>
              </td>
              <td>
                <xsl:value-of select="//registration_date"/>
              </td>
            </tr>
          </table>-->

          <!--<br>
            <xsl:value-of select="concat('Итого:', count(//catalog/book))"/>
          </br>-->
        </li>
      </xsl:for-each>
    </ul>

    <!-- Trailer -->
    <xsl:value-of select="concat('Общий итог по всей библиотеке:', count(/catalog/book))"/>
  </xsl:template>
  
  <!--<xsl:template match="book">
    <xsl:copy-of select="."/>
  </xsl:template>-->

</xsl:stylesheet>